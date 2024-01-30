namespace Application.Features.Streams.Dtos;

public record GetStreamViewerInfoDto(GetStreamDto Stream, bool IsUserBlocked);