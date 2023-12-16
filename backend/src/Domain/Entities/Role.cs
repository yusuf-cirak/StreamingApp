namespace Domain.Entities;

public class Role : Entity
{
    public string Name { get; set; }
    
    public virtual ICollection<UserRoleClaim> UserRoleClaims { get; }
    
    private Role()
    {
    }
    
    private Role(Guid id, string name) : base(id)
    {
        Name = name;
        UserRoleClaims = new HashSet<UserRoleClaim>();
    }
    
    public static Role Create(Guid id, string name)
    {
        var userRole = new Role(id, name);
        //userRole.Raise(new UserRoleCreatedEvent(userRole));
        return userRole;
    }
}