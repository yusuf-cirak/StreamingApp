namespace Application.Features.StreamOptions.Dtos;

public readonly record struct GetStreamChatSettingsDto(
    Guid StreamerId,
    bool ChatDisabled,
    bool MustBeFollower,
    int ChatDelaySecond);

