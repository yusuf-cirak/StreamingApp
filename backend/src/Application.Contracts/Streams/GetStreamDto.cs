using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed record GetStreamDto
{
    public Guid? Id { get; set; }
    public DateTime? StartedAt { get; set; }
    public GetUserDto User { get; set; }
    public GetStreamOptionDto? StreamOption { get; set; }

    public GetStreamDto(Guid id, DateTime startedAt, GetUserDto user, GetStreamOptionDto? streamOption)
    {
        Id = id;
        StartedAt = startedAt;
        User = user;
        StreamOption = streamOption;
    }

    public GetStreamDto(GetUserDto user, GetStreamOptionDto? streamOption)
    {
        User = user;
        StreamOption = streamOption;
    }
}