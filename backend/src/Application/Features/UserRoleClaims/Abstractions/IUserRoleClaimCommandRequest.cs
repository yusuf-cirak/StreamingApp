namespace Application.Features.UserRoleClaims.Abstractions;

public interface IUserRoleClaimCommandRequest
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public string Value { get; set; }
}