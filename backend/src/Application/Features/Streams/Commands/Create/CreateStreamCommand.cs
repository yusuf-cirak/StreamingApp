using Application.Features.Streamers.Abstractions;
using Application.Features.Streams.Rules;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Commands.Create;

public readonly record struct CreateStreamCommandRequest : IStreamerCommandRequest, IRequest<HttpResult<Guid>>,
    ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public CreateStreamCommandRequest()
    {
        // TODO: Add authorization with API key and Stream Key
        AuthorizationRules = [];
    }
}

public sealed class CreateStreamCommandHandler : IRequestHandler<CreateStreamCommandRequest, HttpResult<Guid>>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateStreamCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult<Guid>> Handle(CreateStreamCommandRequest request, CancellationToken cancellationToken)
    {
        var stream = Stream.Create(request.StreamerId);

        _efRepository.Streams.Add(stream);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);
        return result > 0
            ? HttpResult<Guid>.Success(stream.Id, StatusCodes.Status201Created)
            : StreamErrors.FailedToCreateStream;
    }
}