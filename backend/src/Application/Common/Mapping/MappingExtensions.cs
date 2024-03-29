﻿using Application.Features.OperationClaims.Dtos;
using Application.Features.RoleOperationClaims.Dtos;
using Application.Features.Roles.Dtos;
using Application.Features.StreamOptions.Dtos;
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

    public static GetStreamOptionDto ToStreamOptionDto(this StreamOption streamOption) =>
        new(streamOption.StreamTitle, streamOption.StreamDescription, streamOption.ChatDisabled,
            streamOption.ChatDelaySecond, streamOption.StreamKey);

    public static GetStreamChatSettingsDto ToStreamChatSettingsDto(this StreamOption streamOption) =>
        new(streamOption.Id, streamOption.ChatDisabled, streamOption.MustBeFollower, streamOption.ChatDelaySecond);

    public static GetStreamTitleDescriptionDto ToStreamTitleDescriptionDto(this StreamOption streamOption) =>
        new(streamOption.Id, streamOption.StreamTitle, streamOption.StreamDescription);

    public static GetStreamDto ToDto(this Stream stream, Guid streamId, GetUserDto userDto,
        GetStreamOptionDto? streamOptionDto) =>
        new(streamId, stream.StartedAt, userDto, streamOptionDto);
}