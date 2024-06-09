using SharedKernel;

namespace Application.Contracts.Streams;

public sealed record GetStreamInfoDto(GetStreamDto? Stream, Error? Error);