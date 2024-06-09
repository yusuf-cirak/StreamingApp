namespace Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

    public virtual ICollection<RefreshToken> RefreshTokens { get; }

    public virtual ICollection<UserRoleClaim> UserRoleClaims { get; set; } = [];

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = [];

    public virtual ICollection<Stream> Streams { get; }

    public virtual StreamOption StreamOption { get; }


    public User()
    {
        RefreshTokens = new HashSet<RefreshToken>();
        UserRoleClaims = new HashSet<UserRoleClaim>();
        UserOperationClaims = new HashSet<UserOperationClaim>();
        Streams = new HashSet<Stream>();
    }

    private User(string username, byte[] passwordHash, byte[] passwordSalt) : this()
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