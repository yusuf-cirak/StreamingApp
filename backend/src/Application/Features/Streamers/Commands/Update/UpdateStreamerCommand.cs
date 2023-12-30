using System.Security.Claims;
using Application.Common.Extensions;
using Application.Features.Streamers.Abstractions;
using Application.Features.Streamers.Rules;

namespace Application.Features.Streamers.Commands.Update;

public readonly record struct UpdateStreamerCommandRequest
    : IStreamerCommandRequest, IRequest<Result>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public string StreamTitle { get; init; }
    public string StreamDescription { get; init; }
    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public UpdateStreamerCommandRequest()
    {
        AuthorizationRules = new List<Func<ICollection<Claim>, object, Result>>
        {
            StreamerAuthorizationRules.CanUserUpdateStreamer
        };
    }

    public UpdateStreamerCommandRequest(Guid streamerId, string streamTitle, string streamDescription) : this()
    {
        StreamerId = streamerId;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
    }
}

public sealed class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommandRequest, Result>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateStreamerCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(UpdateStreamerCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        await _efRepository.Streamers
            .Where(st => st.Id == userId)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamTitle, x => request.StreamTitle)
                    .SetProperty(x => x.StreamDescription, x => request.StreamDescription),
                cancellationToken: cancellationToken);

        return Result.Success();
    }
}