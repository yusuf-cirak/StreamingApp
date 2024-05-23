using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;
using Application.Features.StreamOptions.Services;
using Application.Features.Streams.Services;

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
    private readonly IStreamOptionService _streamOptionService;

    public UpdateChatSettingsCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor,
        IStreamOptionService streamOptionService)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
        _streamOptionService = streamOptionService;
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

        streamOptions.Update(request.MustBeFollower,
            request.ChatDisabled, request.ChatDelaySecond);


        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        if (result is 0)
        {
            return HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
        }


        _ = _streamOptionService.UpdateStreamOptionCacheAndSendNotificationAsync(streamOptions,
            cancellationToken: cancellationToken);

        return HttpResult.Success(StatusCodes.Status204NoContent);
    }
}