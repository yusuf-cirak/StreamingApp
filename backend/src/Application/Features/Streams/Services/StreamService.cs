using Application.Common.Mapping;
using SignalR.Hubs.Stream.Server.Abstractions;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;


public interface IStreamService : IDomainService<Stream>
{
    IAsyncEnumerable<GetStreamDto> GetRecommendedStreamersAsyncEnumerable();
    Task<List<GetUserDto>> GetModeratingStreamsAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default);

    Result IsStreamerLive(User user, string streamKey);

    Task<GetStreamInfoDto> GetLiveStreamerByNameAsync(string streamerName,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GetStreamDto> FindStreamersByNameAsyncEnumerable(string term);

    Result<(GetStreamDto, int), Error> GetLiveStreamerByKeyFromCache(string streamerKey,
        CancellationToken cancellationToken = default);

    Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default);

    Task<bool> EndStreamAsync(int index, CancellationToken cancellationToken = default);

    Task AddToCacheAndSendNotificationAsync(GetStreamDto streamDto);

    Task<bool> AddToCacheAsync(GetStreamDto streamDto);

    string GenerateStreamKey(User user);
}

public sealed class StreamService(
    IEncryptionHelper encryptionHelper,
    IEfRepository efRepository,
    IStreamCacheService streamCacheService,
    IStreamHubServerService hubServerService)
    : IStreamService
{
    public List<GetStreamDto> LiveStreamers { get; } = streamCacheService.LiveStreamers;


    public IAsyncEnumerable<GetStreamDto> GetRecommendedStreamersAsyncEnumerable()
    {
        var liveStreamers = LiveStreamers;
        var streamersByMostFollowers = efRepository
            .StreamFollowerUsers
            .GroupBy(sfu => sfu.StreamerId)
            .Select(g => new
            {
                StreamerId = g.Key,
                FollowerCount = g.Count()
            })
            .OrderByDescending(g => g.FollowerCount)
            .Take(10);

        var streamers = efRepository
            .Users
            .Include(u => u.StreamOption)
            .Join(streamersByMostFollowers,
                user => user.Id,
                s => s.StreamerId,
                (user, _) => user)
            .Select(user => user.ResolveGetStreamDto(liveStreamers));

        return streamers.AsAsyncEnumerable();
    }

    public Task<List<GetUserDto>> GetModeratingStreamsAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Step 1: Construct the query for UserRoleClaims and UserOperationClaims
        var userClaims = efRepository.UserRoleClaims
            .Where(urc => urc.UserId == userId)
            .Select(urc => urc.Value)
            .Union(
                efRepository.UserOperationClaims
                    .Where(uoc => uoc.UserId == userId)
                    .Select(uoc => uoc.Value)
            )
            .Distinct();

        // Step 2: Query Users table using the combined unique user IDs
        var moderatingStreamers = efRepository.Users
            .Where(u => userClaims.Contains(u.Id.ToString()))
            .Select(u => u.ToDto());



        return moderatingStreamers.ToListAsync(cancellationToken);
    }


    public async Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default)
    {
        var streamer = await efRepository
            .StreamOptions
            .Include(s => s.Streamer)
            .Where(s => s.StreamKey == streamKey)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (streamer is null)
        {
            return StreamOptionErrors.StreamerDoesNotExist;
        }

        return streamer;
    }

    public Result IsStreamerLive(User user, string streamKey)
    {
        var isStreamerLive =
            LiveStreamers.Exists(ls => ls.StreamOption!.Value.StreamKey == streamKey || ls.User.Id == user.Id);

        return isStreamerLive ? Result.Failure(StreamErrors.StreamIsLive) : Result.Success();
    }

    public async Task<GetStreamInfoDto> GetLiveStreamerByNameAsync(string streamerName,
        CancellationToken cancellationToken = default)
    {
        var liveStream = LiveStreamers.SingleOrDefault(stream => stream.User.Username == streamerName);

        if (liveStream is not null)
        {
            return new GetStreamInfoDto(liveStream, null);
        }

        var streamerOption = await efRepository
            .StreamOptions
            .Include(s => s.Streamer)
            .Where(s => s.Streamer.Username == streamerName)
            .SingleOrDefaultAsync(cancellationToken);


        if (streamerOption is null)
        {
            // return StreamErrors.StreamerNotExists;
            return new GetStreamInfoDto(null, StreamErrors.StreamerDoesNotExist);
        }

        return new GetStreamInfoDto(streamerOption.ToDtoWithoutStream(), StreamErrors.StreamIsNotLive);
    }

    public IAsyncEnumerable<GetStreamDto> FindStreamersByNameAsyncEnumerable(string term)
    {
        var liveStreamers = LiveStreamers;

        var streamers = efRepository
            .Users
            .Include(u => u.StreamOption)
            .Where(u => EF.Functions.Like(u.Username, $"%{term}%"))
            .Select(user => user.ResolveGetStreamDto(liveStreamers));

        return streamers.AsAsyncEnumerable();
    }


    public Result<(GetStreamDto, int), Error> GetLiveStreamerByKeyFromCache(string streamerKey,
        CancellationToken cancellationToken = default)
    {
        int index = LiveStreamers.FindIndex(ls => ls.StreamOption!.Value.StreamKey == streamerKey);


        if (index is not -1)
        {
            return (LiveStreamers[index], index);
        }

        return StreamErrors.StreamIsNotLive;
    }

    public async Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default)
    {
        efRepository.Streams.Add(stream);

        var addStream = await efRepository.SaveChangesAsync(cancellationToken);

        if (addStream is 0)
        {
            return false;
        }

        var newStreamKey = GenerateStreamKey(streamOption.Streamer);

        var updateStreamKeyResult = await efRepository.StreamOptions
            .Where(st => st.Id == streamOption.Streamer.Id)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamKey, x => newStreamKey),
                cancellationToken:
                cancellationToken);


        var getStreamDto = stream.ToDto(streamOption.Streamer.ToDto(), streamOption.ToDto());

        _ = Task.Run(() => this.AddToCacheAndSendNotificationAsync(getStreamDto));

        return updateStreamKeyResult > 0;
    }

    public async Task<bool> EndStreamAsync(int index, CancellationToken cancellationToken = default)
    {
        var streamDtoCopy = LiveStreamers[index] with { };
        var setEndDateTask = SetStreamEndDateToDatabaseAsync(streamDtoCopy);
        var removeFromCacheTask = streamCacheService.RemoveStreamFromCacheAsync(index, cancellationToken);

        await Task.WhenAll(removeFromCacheTask, setEndDateTask);

        return removeFromCacheTask.Result && setEndDateTask.Result > 0;
    }

    public async Task AddToCacheAndSendNotificationAsync(GetStreamDto streamDto)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));

        var updateCacheTask = streamCacheService.AddNewStreamToCacheAsync(streamDto);

        var sendNotificationTask = hubServerService.OnStreamStartedAsync(streamDto);

        await Task.WhenAll(updateCacheTask, sendNotificationTask);
    }

    private Task<int> SetStreamEndDateToDatabaseAsync(GetStreamDto streamDto)
    {
        return efRepository
            .Streams
            .Where(s => s.Id == streamDto.Id)
            .ExecuteUpdateAsync(stream => stream
                .SetProperty(s => s.EndedAt, DateTime.UtcNow));
    }

    public string GenerateStreamKey(User streamer)
    {
        var streamKeyText = $"{streamer.Username}-{GenerateRandomText()}";
        return encryptionHelper.Encrypt(streamKeyText);
    }

    public Task<bool> AddToCacheAsync(GetStreamDto streamDto)
    {
        return streamCacheService.AddNewStreamToCacheAsync(streamDto);
    }

    static string GenerateRandomText()
    {
        const int length = 5;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        Random random = new Random();

        var randomChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomChars);
    }
}