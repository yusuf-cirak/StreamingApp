using Application.Features.Users.Services;

namespace Application.Features.Users.Queries.GetByTerm;

public readonly record struct GetUsersByTermQueryRequest(string Term) : IRequest<IEnumerable<GetUserDto>>, ISecuredRequest;

public sealed class GetUsersByTermQueryHandler : IRequestHandler<GetUsersByTermQueryRequest, IEnumerable<GetUserDto>>
{
    private readonly IUserService _userService;

    public GetUsersByTermQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<IEnumerable<GetUserDto>> Handle(GetUsersByTermQueryRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userService.SearchUsersByName(request.Term));
    }
}