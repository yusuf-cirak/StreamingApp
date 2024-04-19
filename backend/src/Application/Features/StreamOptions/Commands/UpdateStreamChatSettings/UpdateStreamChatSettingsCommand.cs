using Application.Abstractions.Caching;
using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Mapping;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace Application.Features.StreamOptions.Commands.Update;

public readonly record struct UpdateStreamChatSettingsCommandRequest
    : IStreamOptionRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public bool ChatDisabled { get; init; } = false;

    public bool MustBeFollower { get; init; }

    public int ChatDelaySecond { get; init; } = 0;

    public AuthorizationFunctions AuthorizationFunctions { get; }

    public UpdateStreamChatSettingsCommandRequest()
    {
        AuthorizationFunctions =
            [StreamOptionAuthorizationRules.CanUserGetOrUpdateStreamOptions];
    }

    public UpdateStreamChatSettingsCommandRequest(Guid streamerId, bool chatDisabled, bool mustBeFollower,
        int chatDelaySecond) : this()
    {
        StreamerId = streamerId;
        ChatDisabled = chatDisabled;
        MustBeFollower = mustBeFollower;
        ChatDelaySecond = chatDelaySecond;
    }
}

public sealed class
    UpdateChatSettingsCommandHandler : IRequestHandler<UpdateStreamChatSettingsCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStreamHubServerService _hubServerService;
    private readonly ICacheService _cacheService;

    public UpdateChatSettingsCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor,
        IStreamHubServerService hubServerService, ICacheService cacheService)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
        _hubServerService = hubServerService;
        _cacheService = cacheService;
    }

    public async Task<HttpResult> Handle(UpdateStreamChatSettingsCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor?.HttpContext?.User.GetUserId()!);

        var streamOptions = await
            _efRepository
                .StreamOptions
                .Include(so => so.Streamer)
                .ThenInclude(user => user.Streams)
                .AsTracking()
                .SingleOrDefaultAsync(so => so.Id == userId,
                    cancellationToken: cancellationToken);


        var liveStreamers = await
            _cacheService.GetOrAddAsync(RedisConstant.Key.LiveStreamers, LiveStreamersFactory,
                cancellationToken: cancellationToken);


        var index = liveStreamers.FindIndex(ls => ls.Id == userId);

        if (index is not -1)
        {
            var previousKey = liveStreamers[index]!.StreamOption!.Value.StreamKey;
            streamOptions.UpdateWithEvent(previousKey, request.MustBeFollower,
                request.ChatDisabled, request.ChatDelaySecond);
            // TODO: Handle the update event, refresh the cache.
            // TODO: Apply locking to the cache when this path occurs.
        }
        else
        {
            streamOptions.Update(request.MustBeFollower, request.ChatDisabled, request.ChatDelaySecond);
        }


        // var result = await _efRepository.StreamOptions
        //     .Where(st => st.Id == userId)
        //     .ExecuteUpdateAsync(
        //         streamer => streamer
        //             .SetProperty(x => x.ChatDisabled, x => request.ChatDisabled)
        //             .SetProperty(x => x.MustBeFollower, x => request.MustBeFollower)
        //             .SetProperty(x => x.ChatDelaySecond, x => request.ChatDelaySecond),
        //         cancellationToken: cancellationToken);


        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        if (result is 0)
        {
            return HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
        }

        var streamerName = _httpContextAccessor.HttpContext.User.Claims
            .Where(claim => claim.Type == ClaimTypes.Name)
            .Select(claim => claim.Value)
            .SingleOrDefault() ?? string.Empty;

        _ = _hubServerService.OnStreamChatOptionsChangedAsync(streamOptions.ToStreamChatSettingsDto(), streamerName);

        return HttpResult.Success(StatusCodes.Status204NoContent);
    }


    private Task<List<GetStreamDto>> LiveStreamersFactory() =>
        _efRepository.StreamOptions.Include(s => s.Streamer)
            .ThenInclude(user => user.Streams)
            .Where(so => so.Streamer.Streams.Any(stream => stream.EndedAt == default))
            .SelectMany(streamOption => streamOption.Streamer.Streams.Take(1)
                .Select(stream => new { Stream = stream, StreamOption = streamOption }))
            .Select(result => result.Stream.ToDto(result.StreamOption.Streamer.ToDto(), result.StreamOption.ToDto()))
            .ToListAsync();

    private async Task RefreshStreamersCacheAsync(StreamOption streamOptions,
        CancellationToken cancellationToken = default)
    {
        // should use Get instead of getoradd
        // move this into somewhere else, listen to DB events, don't create your own
        var liveStreams = await _cacheService.GetOrAddAsync(RedisConstant.Key.LiveStreamers,
            LiveStreamersFactory,
            cancellationToken: cancellationToken);

        var index = liveStreams.FindIndex(ls => ls.Id == streamOptions.Id);

        if (index is -1)
        {
            return;
        }


        var stream = streamOptions.Streamer.Streams.TakeLast(1).SingleOrDefault();

        if (stream == default)
        {
            return;
        }

        var streamDto = stream.ToDto(streamOptions.Streamer.ToDto(), streamOptions.ToDto());


        liveStreams[index] = streamDto;


        await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams,
            cancellationToken: cancellationToken);
    }
}