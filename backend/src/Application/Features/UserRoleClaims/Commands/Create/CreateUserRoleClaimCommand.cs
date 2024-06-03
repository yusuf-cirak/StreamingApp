using Application.Common.Permissions;
using Application.Features.UserRoleClaims.Abstractions;

namespace Application.Features.UserRoleClaims.Commands.Create;

public record struct CreateUserRoleClaimCommandRequest : IUserRoleClaimCommandRequest, IRequest<HttpResult>,
    ISecuredRequest
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
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray());
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class
    CreateUserRoleClaimHandler : IRequestHandler<CreateUserRoleClaimCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    public CreateUserRoleClaimHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(CreateUserRoleClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userRoleClaim = UserRoleClaim.Create(request.UserId, request.RoleId, request.Value);

        _efRepository.UserRoleClaims.Add(userRoleClaim);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status201Created)
            : HttpResult.Failure(UserRoleClaimErrors.FailedToCreate);
    }
}