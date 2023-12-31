using Application.Features.UserOperationClaims.Abstractions;
using Application.Features.UserOperationClaims.Rules;

namespace Application.Features.UserOperationClaims.Commands.Delete;

public readonly record struct DeleteUserOperationClaimCommandRequest : IUserOperationClaimCommandRequest,
    IRequest<HttpResult>, ISecuredRequest
{
    public Guid UserId { get; init; }
    public Guid OperationClaimId { get; init; }
    public string Value { get; init; }

    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public DeleteUserOperationClaimCommandRequest()
    {
        AuthorizationRules = [UserOperationClaimAuthorizationRules.CanUserCreateOrDeleteUserOperationClaim];
    }

    public DeleteUserOperationClaimCommandRequest(Guid userId, Guid operationClaimId, string value)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
        Value = value;
    }
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