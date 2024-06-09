namespace Domain.Entities;

public class Role : Entity
{
    public string Name { get; set; }

    private Role()
    {
    }

    private Role(string name)
    {
        Name = name;
    }

    private Role(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Role Create(string name)
    {
        var userRole = new Role(name);
        //userRole.Raise(new UserRoleCreatedEvent(userRole));
        return userRole;
    }

    public static Role Create(Guid id, string name)
    {
        var userRole = new Role(id, name);
        return userRole;
    }
}