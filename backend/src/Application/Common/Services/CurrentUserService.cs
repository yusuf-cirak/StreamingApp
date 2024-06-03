namespace Application.Common.Services;

public sealed record CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public Guid UserId => Guid.Parse(User.GetUserId());

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}