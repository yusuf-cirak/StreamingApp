namespace Domain.Entities;

public class RefreshToken : Entity
{
    public Guid UserId { get;  }
    public string CreatedByIp { get;  }
    public DateTime ExpiresAt { get;  }
    public string Token { get;  }

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
