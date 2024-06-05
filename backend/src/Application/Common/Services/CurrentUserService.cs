namespace Application.Common.Services;

public interface ICurrentUserService
{
    public ClaimsPrincipal? User { get; }
    public Guid UserId { get; }
}

public sealed record CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public Guid UserId => Guid.Parse(User.GetUserId());

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}