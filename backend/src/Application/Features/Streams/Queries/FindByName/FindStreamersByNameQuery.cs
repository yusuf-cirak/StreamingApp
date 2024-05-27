using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.FindByName;

public readonly record struct FindStreamersByNameQueryRequest(string Term)
    : IRequest<HttpResult<IAsyncEnumerable<GetStreamDto>>>;

public sealed class
    FindStreamersByNameQueryHandler : IRequestHandler<FindStreamersByNameQueryRequest,
    HttpResult<IAsyncEnumerable<GetStreamDto>>>
{
    private readonly IStreamService _streamService;

    public FindStreamersByNameQueryHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public Task<HttpResult<IAsyncEnumerable<GetStreamDto>>> Handle(FindStreamersByNameQueryRequest request,
        CancellationToken cancellationToken)
    {
        var streamersEnumerable = _streamService.FindStreamersByNameAsyncEnumerable(request.Term);

        return Task.FromResult(HttpResult<IAsyncEnumerable<GetStreamDto>>.Success(streamersEnumerable));
    }
}