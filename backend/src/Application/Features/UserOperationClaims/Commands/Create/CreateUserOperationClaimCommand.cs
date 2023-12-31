using Application.Features.UserOperationClaims.Abstractions;
using Application.Features.UserOperationClaims.Rules;

namespace Application.Features.UserOperationClaims.Commands.Create;

public readonly record struct CreateUserOperationClaimCommandRequest : IUserOperationClaimCommandRequest,
    IRequest<HttpResult>, ISecuredRequest
{
    public Guid UserId { get; init; }
    public Guid OperationClaimId { get; init; }
    public string Value { get; init; }

    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public CreateUserOperationClaimCommandRequest()
    {
        AuthorizationRules = [UserOperationClaimAuthorizationRules.CanUserCreateOrDeleteUserOperationClaim];
    }

    public CreateUserOperationClaimCommandRequest(Guid userId, Guid operationClaimId, string value)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
        Value = value;
    }
}

public sealed class
    CreateUserOperationClaimHandler : IRequestHandler<CreateUserOperationClaimCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    public CreateUserOperationClaimHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(CreateUserOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userOperationClaim = UserOperationClaim.Create(request.UserId, request.OperationClaimId, request.Value);

        _efRepository.UserOperationClaims.Add(userOperationClaim);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status201Created)
            : HttpResult.Failure(UserOperationClaimErrors.FailedToCreateUserOperationClaimForValue);
    }
}