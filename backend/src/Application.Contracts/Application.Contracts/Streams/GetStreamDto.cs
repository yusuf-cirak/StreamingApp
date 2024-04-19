using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed class GetStreamDto
{
    public Guid Id { get; }
    public DateTime StartedAt { get; }
    public GetUserDto User { get; }
    public GetStreamOptionDto? StreamOption { get; set; }

    public GetStreamDto(Guid id, DateTime startedAt, GetUserDto user, GetStreamOptionDto? streamOption)
    {
        Id = id;
        StartedAt = startedAt;
        User = user;
        StreamOption = streamOption;
    }
}