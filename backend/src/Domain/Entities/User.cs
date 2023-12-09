namespace Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

    public virtual IList<RefreshToken> RefreshTokens { get; set; }

    private User(string username, byte[] passwordHash, byte[] passwordSalt)
    {
        Username = username;
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