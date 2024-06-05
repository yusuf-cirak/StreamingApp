namespace Domain.Entities;

public class UserRoleClaim : BaseEntity, IEquatable<UserRoleClaim>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public string Value { get; set; }

    public virtual User User { get; set; }
    public virtual Role Role { get; set; }

    public UserRoleClaim()
    {
    }

    private UserRoleClaim(Guid userId, Guid roleId, string value)
    {
        UserId = userId;
        RoleId = roleId;
        Value = value;
    }

    public static UserRoleClaim Create(Guid userId, Guid roleId, string value)
    {
        UserRoleClaim userRoleClaim = new(userId, roleId, value);
        return userRoleClaim;
    }

    public bool Equals(UserRoleClaim other)
    {
        if (other == null) return false;
        return this.RoleId == other.RoleId && this.UserId == other.UserId && this.Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is UserRoleClaim other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, RoleId, Value);
    }
}