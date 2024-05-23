namespace Application.Contracts.StreamOptions;

public readonly record struct GetStreamOptionDto(
    string Title,
    string Description,
    bool ChatDisabled,
    bool MustBeFollower,
    int ChatDelaySecond,
    string? StreamKey,
    string ThumbnailUrl);