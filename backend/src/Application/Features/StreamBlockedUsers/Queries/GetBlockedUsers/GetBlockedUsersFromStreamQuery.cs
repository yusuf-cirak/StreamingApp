using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Rules;
using Application.Features.StreamBlockedUsers.Services;
using Application.Features.Streams.Dtos;

namespace Application.Features.StreamBlockedUsers.Queries.GetBlockedUsers;

public readonly record struct GetBlockedUsersFromStreamQueryRequest :
    IRequest<HttpResult<IAsyncEnumerable<GetStreamBlockedUserDto>>>, IStreamBlockedUserRequest,
    ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GetBlockedUsersFromStreamQueryRequest()
    {
        AuthorizationFunctions = [StreamBlockedUserAuthorizationRules.CanUserBlockOrUnblockAUserFromStream];
    }

    public GetBlockedUsersFromStreamQueryRequest(Guid streamerId) : this()
    {
        StreamerId = streamerId;
    }
}

public sealed class GetBlockedUsersFromStreamQueryHandler : IRequestHandler<GetBlockedUsersFromStreamQueryRequest,
    HttpResult<IAsyncEnumerable<GetStreamBlockedUserDto>>>
{
    private readonly IStreamBlockUserService _streamBlockUserService;

    public GetBlockedUsersFromStreamQueryHandler(IStreamBlockUserService streamBlockUserService)
    {
        _streamBlockUserService = streamBlockUserService;
    }

    public Task<HttpResult<IAsyncEnumerable<GetStreamBlockedUserDto>>> Handle(
        GetBlockedUsersFromStreamQueryRequest request, CancellationToken cancellationToken)
    {
        var res = _streamBlockUserService.GetBlockedUsersOfStreamAsyncEnumerable(request.StreamerId);

        return Task.FromResult(HttpResult<IAsyncEnumerable<GetStreamBlockedUserDto>>.Success(res));
    }
}