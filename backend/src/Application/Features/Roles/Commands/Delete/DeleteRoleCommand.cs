using System.Security.Claims;
using Application.Common.Rules;
using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Delete;

public readonly record struct DeleteRoleCommandRequest
    : IRequest<HttpResult>, ISecuredRequest
{
    public Guid Id { get; init; }
    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; } = new();

    public DeleteRoleCommandRequest()
    {
        AuthorizationRules = [CommonAuthorizationRules.UserMustBeAdmin];
    }

    public DeleteRoleCommandRequest(Guid id) : this()
    {
        Id = id;
    }
}

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleBusinessRules _roleBusinessRules;

    public DeleteRoleCommandHandler(IEfRepository efRepository, RoleBusinessRules roleBusinessRules)
    {
        _efRepository = efRepository;
        _roleBusinessRules = roleBusinessRules;
    }

    public async Task<HttpResult> Handle(DeleteRoleCommandRequest request,
        CancellationToken cancellationToken)
    {
        var roleExistResult = await _roleBusinessRules.RoleMustExistBeforeDeleted(request.Id, cancellationToken);

        if (roleExistResult.IsFailure)
        {
            return roleExistResult.Error;
        }

        var result = await _efRepository.Roles.Where(r => r.Id == request.Id).ExecuteDeleteAsync(cancellationToken);


        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : RoleErrors.FailedToDelete;
    }
}