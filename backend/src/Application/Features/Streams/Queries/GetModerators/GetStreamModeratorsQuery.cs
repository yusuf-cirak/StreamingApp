using Application.Common.Permissions;
using Application.Common.Services;
using Application.Features.Streams.Dtos;
using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.GetModerators;

public readonly record struct GetStreamModeratorsQueryRequest()
    : IRequest<List<GetStreamModeratorDto>>, ISecuredRequest;

public sealed class GetStreamModeratorsQueryRequestHandler(
    ICurrentUserService currentUserService,
    IStreamModeratorService streamModeratorService)
    : IRequestHandler<GetStreamModeratorsQueryRequest, List<GetStreamModeratorDto>>
{
    public async Task<List<GetStreamModeratorDto>> Handle(GetStreamModeratorsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        return await streamModeratorService.GetStreamModeratorsAsync(userId, cancellationToken);
    }
}