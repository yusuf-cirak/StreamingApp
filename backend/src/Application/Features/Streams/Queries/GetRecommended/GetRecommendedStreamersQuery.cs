using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.GetRecommended;

public readonly record struct GetRecommendedStreamersQueryRequest()
    : IRequest<HttpResult<IAsyncEnumerable<GetStreamDto>>>;

public sealed class
    GetRecommendedStreamersQueryHandler : IRequestHandler<GetRecommendedStreamersQueryRequest,
    HttpResult<IAsyncEnumerable<GetStreamDto>>>
{
    private readonly IStreamService _streamService;

    public GetRecommendedStreamersQueryHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public Task<HttpResult<IAsyncEnumerable<GetStreamDto>>> Handle(GetRecommendedStreamersQueryRequest request,
        CancellationToken cancellationToken)
        => Task.FromResult(
            HttpResult<IAsyncEnumerable<GetStreamDto>>.Success(_streamService
                .GetRecommendedStreamersAsyncEnumerable()));
}