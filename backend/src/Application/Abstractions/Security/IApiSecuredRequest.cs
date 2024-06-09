namespace Application.Abstractions.Security;

public interface IApiSecuredRequest
{
    public AuthorizationRequirements AuthorizationRequirements { get; }
}