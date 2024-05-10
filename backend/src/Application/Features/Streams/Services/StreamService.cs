using Application.Common.Mapping;
using SignalR.Hubs.Stream.Server.Abstractions;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public sealed class StreamService : IStreamService
{
    public List<GetStreamDto> LiveStreamers { get; }

    private readonly IEncryptionHelper _encryptionHelper;
    private readonly IEfRepository _efRepository;
    private readonly IStreamCacheService _streamCacheService;
    private readonly IStreamHubServerService _hubServerService;


    public StreamService(IEncryptionHelper encryptionHelper, IEfRepository efRepository,
        IStreamCacheService streamCacheService, IStreamHubServerService hubServerService)
    {
        _encryptionHelper = encryptionHelper;
        _efRepository = efRepository;
        _streamCacheService = streamCacheService;
        _hubServerService = hubServerService;
        LiveStreamers = streamCacheService.LiveStreamers;
    }


    public async Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default)
    {
        var streamer = await _efRepository
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

        var streamerOption = await _efRepository
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

    public Task<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository.StreamFollowerUsers
            .Include(s => s.Streamer)
            .Where(sfu => sfu.UserId == userId)
            .Select(sfu => new GetFollowingStreamDto(sfu.Streamer.ToDto())).ToListAsync(cancellationToken);
    }

    public async Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default)
    {
        _efRepository.Streams.Add(stream);

        var addStream = await _efRepository.SaveChangesAsync(cancellationToken);

        if (addStream is 0)
        {
            return false;
        }

        var newStreamKey = GenerateStreamKey(streamOption.Streamer);

        var updateStreamKeyResult = await _efRepository.StreamOptions
            .Where(st => st.Id == streamOption.Streamer.Id)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamKey, x => newStreamKey),
                cancellationToken:
                cancellationToken);


        var getStreamDto = stream.ToDto(streamOption.Streamer.ToDto(), streamOption.ToDto());

        _ = this.AddToCacheAndSendNotificationAsync(getStreamDto);

        return updateStreamKeyResult > 0;
    }

    public async Task<bool> EndStreamAsync(int index, CancellationToken cancellationToken = default)
    {
        var streamDtoCopy = LiveStreamers[index] with { };
        var setEndDateTask = SetStreamEndDateToDatabaseAsync(streamDtoCopy);
        var removeFromCacheTask = _streamCacheService.RemoveStreamFromCacheAsync(index, cancellationToken);

        await Task.WhenAll(removeFromCacheTask, setEndDateTask);

        return removeFromCacheTask.Result && setEndDateTask.Result > 0;
    }

    public async Task AddToCacheAndSendNotificationAsync(GetStreamDto streamDto)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));

        var updateCacheTask = _streamCacheService.AddNewStreamToCacheAsync(streamDto);

        var sendNotificationTask = _hubServerService.OnStreamStartedAsync(streamDto);

        await Task.WhenAll(updateCacheTask, sendNotificationTask);
    }

    private Task<int> SetStreamEndDateToDatabaseAsync(GetStreamDto streamDto)
    {
        return _efRepository
            .Streams
            .Where(s => s.Id == streamDto.Id)
            .ExecuteUpdateAsync(stream => stream
                .SetProperty(s => s.EndedAt, DateTime.UtcNow));
    }

    public string GenerateStreamKey(User streamer)
    {
        var streamKeyText = $"{streamer.Username}-{GenerateRandomText()}";
        return _encryptionHelper.Encrypt(streamKeyText);
    }

    public Task<bool> AddToCacheAsync(GetStreamDto streamDto)
    {
        return _streamCacheService.AddNewStreamToCacheAsync(streamDto);
    }

    public async Task
        UpdateStreamOptionCacheAndSendNotificationAsync(StreamOption streamOption, CancellationToken cancellationToken =
            default)
    {
        var index = LiveStreamers.FindIndex(ls => ls.User.Id == streamOption.Streamer.Id);

        if (index is -1)
        {
            return;
        }

        var currentState = LiveStreamers[index].StreamOption!.Value;

        LiveStreamers[index].StreamOption = streamOption.ToDto(streamKey: currentState.StreamKey);

        var updateCacheTask = _streamCacheService.SetLiveStreamsAsync(LiveStreamers, cancellationToken);
        var sendNotificationTask =
            this.SendChatOptionsUpdatedNotificationAsync(LiveStreamers[index], cancellationToken);

        await Task.WhenAll(updateCacheTask, sendNotificationTask);
    }

    public Task SendChatOptionsUpdatedNotificationAsync(GetStreamDto streamDto,
        CancellationToken cancellationToken = default)
    {
        return _hubServerService.OnStreamChatOptionsChangedAsync(
            streamDto.StreamOption!.Value.ToStreamChatSettingsDto(), streamDto.User.Username);
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