namespace Application.Features.StreamOptions.Dtos;

public readonly record struct GetStreamOptionDto(string Title, string Description, bool ChatDisabled, int ChatDelaySecond);