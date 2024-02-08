namespace Application.Features.StreamFollowerUsers.Services;

public interface IStreamFollowerUserService : IDomainService<StreamFollowerUser>
{
    Task<bool> IsUserFollowingStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken);
}