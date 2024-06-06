namespace Application.Common.Services;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext!.User.GetUserId());

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}