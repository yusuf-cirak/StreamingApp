using Application.Common.Permissions;
using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Services;
using Application.Features.Streams.Dtos;

namespace Application.Features.StreamBlockedUsers.Queries.GetBlockedUsers;

public record struct GetBlockedUsersFromStreamQueryRequest :
    IRequest<HttpResult<IAsyncEnumerable<GetStreamBlockedUserDto>>>, IStreamBlockedUserRequest,
    IPermissionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;

            this.PermissionRequirements = PermissionRequirements
                .Create()
                .WithRequiredValue(value.ToString())
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
                .WithOperationClaims(RequiredClaim.Create(OperationClaimConstants.Stream.Read.BlockFromChat,
                    StreamErrors.UserIsNotModeratorOfStream), RequiredClaim.Create(
                    OperationClaimConstants.Stream.Write.BlockFromChat,
                    StreamErrors.UserIsNotModeratorOfStream))
                .WithNameIdentifierClaim();
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }


    public GetBlockedUsersFromStreamQueryRequest(Guid streamerId)
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