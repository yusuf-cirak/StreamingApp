namespace Application.Contracts.StreamOptions;

public readonly record struct GetStreamChatSettingsDto(
    // Guid StreamerId,
    bool ChatDisabled,
    bool MustBeFollower,
    int ChatDelaySecond);

