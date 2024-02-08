namespace Application.Features.StreamOptions.Dtos;

public readonly record struct GetStreamTitleDescriptionDto(Guid StreamerId, string Title, string Description);