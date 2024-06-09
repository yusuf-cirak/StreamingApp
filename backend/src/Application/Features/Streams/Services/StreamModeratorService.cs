using Application.Features.Streams.Dtos;

namespace Application.Features.Streams.Services;

public interface IStreamModeratorService : IDomainService<User>
{
    Task<List<GetStreamModeratorDto>>
        GetStreamModeratorsAsync(Guid streamerId, CancellationToken cancellationToken = default);
}

public sealed class StreamModeratorService(IEfRepository efRepository) : IStreamModeratorService
{
    public async Task<List<GetStreamModeratorDto>> GetStreamModeratorsAsync(Guid streamerId,
        CancellationToken cancellationToken = default)
    {
        var streamerIdString = streamerId.ToString();

        var moderators = await efRepository
            .Users
            .Where(u => u.UserRoleClaims.Any(urc => urc.Value == streamerIdString) ||
                        u.UserOperationClaims.Any(uoc => uoc.Value == streamerIdString))
            .Select(u => new GetStreamModeratorDto
            {
                Id = u.Id,
                Username = u.Username,
                ProfileImageUrl = u.ProfileImageUrl,
                RoleIds = u.UserRoleClaims.Where(urc => urc.Value == streamerIdString).Select(urc => urc.RoleId)
                    .ToList(),
                OperationClaimIds = u.UserOperationClaims.Where(urc => urc.Value == streamerIdString)
                    .Select(urc => urc.OperationClaimId).ToList(),
            })
            .ToListAsync(cancellationToken);

        return moderators;
    }
}