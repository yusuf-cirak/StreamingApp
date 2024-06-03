using Application.Common.Permissions;

namespace Application.Abstractions.Security;

public interface ISecuredRequest
{
    PermissionRequirements PermissionRequirements { get; }
}