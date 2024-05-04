namespace Application.Contracts.StreamOptions;

public readonly record struct GetStreamTitleDescriptionDto(Guid StreamerId, string Title, string Description);