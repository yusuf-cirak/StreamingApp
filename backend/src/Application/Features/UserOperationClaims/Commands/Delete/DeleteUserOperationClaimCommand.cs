using Application.Common.Permissions;
using Application.Features.UserOperationClaims.Abstractions;
using Application.Features.UserOperationClaims.Rules;

namespace Application.Features.UserOperationClaims.Commands.Delete;

public record struct DeleteUserOperationClaimCommandRequest : IUserOperationClaimCommandRequest,
    IRequest<HttpResult>, IPermissionRequest
{
    public Guid UserId { get; init; }
    public Guid OperationClaimId { get; init; }
    private string _value;

    public string Value
    {
        get => _value;
        set
        {
            _value = value;

            PermissionRequirements = PermissionRequirements.Create()
                .WithRequiredValue(value)
                .WithNameIdentifierClaim();
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class
    DeleteUserOperationClaimHandler : IRequestHandler<DeleteUserOperationClaimCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    public DeleteUserOperationClaimHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(DeleteUserOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _efRepository
            .UserOperationClaims
            .Where(uoc => uoc.Value == request.Value && uoc.UserId == request.UserId &&
                          uoc.OperationClaimId == request.OperationClaimId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(UserOperationClaimErrors.FailedToDeleteUserOperationClaimForValue);
    }
}