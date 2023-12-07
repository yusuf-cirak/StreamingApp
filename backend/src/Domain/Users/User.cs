
namespace Domain.Users;

public sealed class User : AuditableEntity
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

    private User(string userName, byte[] passwordHash, byte[] passwordSalt)
    {
        Username = userName;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public static User Create(string userName, byte[] passwordHash, byte[] passwordSalt)
    {
        User user = new(userName, passwordHash, passwordSalt);

        user.Raise(new UserCreatedEvent(user));

        return user;
    }
}