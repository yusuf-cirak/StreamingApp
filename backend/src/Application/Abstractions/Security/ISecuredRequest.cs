namespace Application.Abstractions.Security;

public interface ISecuredRequest
{
    AuthorizationFunctions AuthorizationFunctions { get; }
}