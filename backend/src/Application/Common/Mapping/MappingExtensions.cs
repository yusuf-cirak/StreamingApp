using Application.Features.Streams.Dtos;
using Stream = Domain.Entities.Stream;

namespace Application.Common.Mapping;

public static class MappingExtensions
{
    public static GetOperationClaimDto ToDto(this OperationClaim operationClaim) =>
        new(operationClaim.Id, operationClaim.Name);

    public static GetRoleDto ToDto(this Role role) => new(role.Id, role.Name);

    public static GetRoleOperationClaimDto ToDto(this RoleOperationClaim roleOperationClaim) =>
        new(roleOperationClaim.RoleId, roleOperationClaim.OperationClaimId);

    public static GetUserRoleDto ToDto(this UserRoleClaim userRoleClaim, string roleName = null) =>
        new(roleName ?? userRoleClaim.Role.Name, userRoleClaim.Value);

    public static GetUserOperationClaimDto
        ToDto(this UserOperationClaim userOperationClaim, string roleName = null) =>
        new(roleName ?? userOperationClaim.OperationClaim.Name,
            userOperationClaim.Value);

    public static GetUserDto ToDto(this User user) =>
        new(user.Id, user.Username, user.ProfileImageUrl);

    public static GetStreamOptionDto ToDto(this StreamOption streamOption, bool withoutKey = false,
        string streamKey = null) =>
        new(streamOption.StreamTitle, streamOption.StreamDescription, streamOption.ChatDisabled,
            streamOption.MustBeFollower,
            streamOption.ChatDelaySecond, withoutKey is false ? streamKey ?? streamOption.StreamKey : null,
            streamOption.ThumbnailUrl);

    public static GetStreamChatSettingsDto ToStreamChatSettingsDto(this StreamOption streamOption) =>
        new(streamOption.ChatDisabled, streamOption.MustBeFollower, streamOption.ChatDelaySecond);

    public static GetStreamChatSettingsDto ToStreamChatSettingsDto(this GetStreamOptionDto streamOption) =>
        new(streamOption.ChatDisabled, streamOption.MustBeFollower, streamOption.ChatDelaySecond);

    public static GetStreamTitleDescriptionDto ToStreamTitleDescriptionDto(this StreamOption streamOption) =>
        new(streamOption.Id, streamOption.StreamTitle, streamOption.StreamDescription);

    public static GetStreamDto ToDto(this Stream stream, GetUserDto userDto,
        GetStreamOptionDto? streamOptionDto) =>
        new(stream.Id, stream.StartedAt, userDto, streamOptionDto);

    public static GetStreamDto ToDtoWithoutStream(this StreamOption streamOption) =>
        new(streamOption.Streamer.ToDto(), streamOption.ToDto(true));


    public static GetStreamDto ResolveGetStreamDto(this User user, List<GetStreamDto> liveStreamers)
    {
        var index = liveStreamers.FindIndex(ls => ls.User.Id == user.Id);

        return new GetStreamDto(user.ToDto(),
            user.StreamOption.ToDto(index is -1,
                index is not -1 ? liveStreamers[index].StreamOption!.Value.StreamKey : null));
    }


    public static GetStreamBlockedUserDto ToStreamBlockedUserDto(this User user, DateTime blockedAt) =>
        new(user.Id, user.Username, user.ProfileImageUrl, blockedAt);

}