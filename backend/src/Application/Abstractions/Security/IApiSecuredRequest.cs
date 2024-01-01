namespace Application.Abstractions.Security;

public interface IApiSecuredRequest
{
    public AuthorizationFunctions AuthorizationFunctions { get; }
}