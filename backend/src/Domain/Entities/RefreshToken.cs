namespace Domain.Entities;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Token { get; set; }

    public virtual User User { get; set; }

    private RefreshToken()
    {

    }

    private RefreshToken(string token, Guid userId, string createdByIp, DateTime expiresAt)
    {
        Token = token;
        UserId = userId;
        CreatedByIp = createdByIp;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(string token, Guid userId, string createdByIp, DateTime expiresAt)
    {
        RefreshToken refreshToken = new(token, userId, createdByIp, expiresAt);
        refreshToken.Raise(new RefreshTokenCreatedEvent(refreshToken));
        return refreshToken;
    }
}
