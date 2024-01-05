namespace Application.Features.Streamers.Dtos;

public readonly record struct GetStreamerDto(string Title, string Description, bool ChatDisabled, int ChatDelaySecond);