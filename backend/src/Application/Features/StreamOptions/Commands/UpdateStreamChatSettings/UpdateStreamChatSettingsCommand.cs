using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;

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

public sealed class UpdateChatSettingsCommandHandler : IRequestHandler<UpdateStreamChatSettingsCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateChatSettingsCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult> Handle(UpdateStreamChatSettingsCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        var result = await _efRepository.StreamOptions
            .Where(st => st.Id == userId)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.ChatDisabled, x => request.ChatDisabled)
                    .SetProperty(x => x.MustBeFollower, x => request.MustBeFollower)
                    .SetProperty(x => x.ChatDelaySecond, x => request.ChatDelaySecond),
                cancellationToken: cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
    }
}