using Application.Features.OperationClaims.Dtos;
using Application.Features.RoleOperationClaims.Dtos;
using Application.Features.Roles.Dtos;
using Application.Features.Streamers.Dtos;
using Application.Features.Streams.Dtos;
using Application.Features.Users.Dtos;
using Stream = Domain.Entities.Stream;

namespace Application.Common.Mapping;

public static class MappingExtensions
{
    public static GetOperationClaimDto ToDto(this OperationClaim operationClaim) =>
        new(operationClaim.Id, operationClaim.Name);

    public static GetRoleDto ToDto(this Role role) => new(role.Id, role.Name);

    public static GetRoleOperationClaimDto ToDto(this RoleOperationClaim roleOperationClaim) =>
        new(roleOperationClaim.RoleId, roleOperationClaim.OperationClaimId);

    public static GetUserDto ToDto(this User user) =>
        new(user.Id, user.Username, user.ProfileImageUrl);

    public static GetStreamerDto ToDto(this Streamer streamer) =>
        new(streamer.StreamTitle, streamer.StreamDescription, streamer.ChatDisabled, streamer.ChatDelaySecond);

    public static GetStreamDto ToDto(this Stream stream, GetUserDto userDto, GetStreamerDto streamerDto) =>
        new(stream.Id, stream.StartedAt, userDto, streamerDto);
}