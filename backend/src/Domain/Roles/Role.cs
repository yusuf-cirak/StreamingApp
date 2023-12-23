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

    public static Role Create(string name)
    {
        var userRole = new Role(name);
        //userRole.Raise(new UserRoleCreatedEvent(userRole));
        return userRole;
    }
}