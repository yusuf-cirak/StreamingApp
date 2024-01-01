using Application.Features.UserRoleClaims.Abstractions;
using Application.Features.UserRoleClaims.Rules;

namespace Application.Features.UserRoleClaims.Commands.Delete;

public readonly record struct DeleteUserRoleClaimCommandRequest : IUserRoleClaimCommandRequest, IRequest<HttpResult>,
    ISecuredRequest
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public string Value { get; init; }

    public AuthorizationFunctions AuthorizationFunctions { get; }

    public DeleteUserRoleClaimCommandRequest()
    {
        AuthorizationFunctions = [UserRoleClaimAuthorizationRules.CanUserCreateOrDeleteUserRoleClaim];
    }


    public DeleteUserRoleClaimCommandRequest(Guid userId, Guid roleId, string value) : this()
    {
        UserId = userId;
        RoleId = roleId;
        Value = value;
    }
}

public sealed class DeleteUserRoleClaimHandler : IRequestHandler<DeleteUserRoleClaimCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    public DeleteUserRoleClaimHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(DeleteUserRoleClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _efRepository
            .UserRoleClaims
            .Where(urc => urc.Value == request.Value && urc.UserId == request.UserId && urc.RoleId == request.RoleId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(UserRoleClaimErrors.FailedToDelete);
    }
}