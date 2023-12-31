using Application.Features.UserRoleClaims.Abstractions;
using Application.Features.UserRoleClaims.Rules;

namespace Application.Features.UserRoleClaims.Commands.Create;

public readonly record struct CreateUserRoleClaimCommandRequest : IUserRoleClaimCommandRequest, IRequest<HttpResult>,
    ISecuredRequest
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public string Value { get; init; }

    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public CreateUserRoleClaimCommandRequest()
    {
        AuthorizationRules = [UserRoleClaimAuthorizationRules.CanUserCreateOrDeleteUserRoleClaim];
    }

    public CreateUserRoleClaimCommandRequest(Guid userId, Guid roleId, string value) : this()
    {
        UserId = userId;
        RoleId = roleId;
        Value = value;
    }
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