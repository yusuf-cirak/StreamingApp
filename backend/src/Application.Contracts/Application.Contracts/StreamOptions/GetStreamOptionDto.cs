namespace Application.Contracts.StreamOptions;

public readonly record struct GetStreamOptionDto(
    string Title,
    string Description,
    bool ChatDisabled,
    int ChatDelaySecond,
    string StreamKey);