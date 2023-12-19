namespace Application.Abstractions.Security;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AuthorizationPipelineAttribute : Attribute
{
    /// <summary>
    /// Define rules separating with commas
    /// </summary>
    public string[] Roles { get; set; }

}