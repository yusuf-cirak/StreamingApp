using Application.Common.Permissions;

namespace Application.Abstractions.Security;

public interface IPermissionRequest : ISecuredRequest, ISensitiveRequest
{
    PermissionRequirements PermissionRequirements { get; }
}