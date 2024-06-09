namespace Application.Common.Services;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            ArgumentNullException.ThrowIfNull(user, nameof(httpContextAccessor.HttpContext.User));

            return Guid.Parse(user.GetUserId());
        }
    }
}