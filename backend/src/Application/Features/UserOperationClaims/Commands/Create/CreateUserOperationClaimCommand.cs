using Application.Common.Permissions;
using Application.Features.UserOperationClaims.Abstractions;

namespace Application.Features.UserOperationClaims.Commands.Create;

public record struct CreateUserOperationClaimCommandRequest : IUserOperationClaimCommandRequest,
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

            this.PermissionRequirements = PermissionRequirements.Create()
                .WithRequiredValue(value)
                .WithNameIdentifierClaim();
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }
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