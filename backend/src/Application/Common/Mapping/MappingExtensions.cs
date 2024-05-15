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

    public static GetStreamOptionDto ToDto(this StreamOption streamOption, bool withoutKey = false,
        string streamKey = null) =>
        new(streamOption.StreamTitle, streamOption.StreamDescription, streamOption.ChatDisabled,
            streamOption.MustBeFollower,
            streamOption.ChatDelaySecond, withoutKey is false ? streamKey ?? streamOption.StreamKey : null);

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
}