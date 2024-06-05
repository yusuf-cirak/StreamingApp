using Application.Common.Permissions;
using Application.Features.UserRoleClaims.Abstractions;

namespace Application.Features.UserRoleClaims.Commands.Delete;

public record struct DeleteUserRoleClaimCommandRequest : IUserRoleClaimCommandRequest, IRequest<HttpResult>,
    IPermissionRequest
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    private string _value;

    public string Value
    {
        get => _value;
        set
        {
            _value = value;

            this.PermissionRequirements = PermissionRequirements.Create()
                .WithRequiredValue(value)
                .WithNameIdentifierClaim();
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }
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