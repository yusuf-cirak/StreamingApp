using Application.Abstractions;

namespace Application.Features.Roles.Rules;

public sealed class RoleBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public RoleBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<Result> RoleNameMustBeUnique(string name, CancellationToken cancellationToken)
    {
        var role = await _efRepository.Roles.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (role is not null)
        {
            return Result.Failure(RoleErrors.RoleAlreadyExists(name));
        }

        return Result.Success();
    }

    public async Task<Result> RoleMustExistBeforeDeleted(Guid id, CancellationToken cancellationToken)
    {
        var role = await _efRepository.Roles.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (role is null)
        {
            return Result.Failure(RoleErrors.RoleCannotBeDeleted(id));
        }

        return Result.Success();
    }
}