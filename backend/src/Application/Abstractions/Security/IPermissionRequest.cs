using Application.Common.Permissions;

namespace Application.Abstractions.Security;

public interface IPermissionRequest : ISecuredRequest
{
    PermissionRequirements PermissionRequirements { get; }
}