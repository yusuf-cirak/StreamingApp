using System.Security.Claims;

namespace Application.Abstractions.Security;

public interface ISecuredRequest
{
    List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }
}