using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed record GetStreamDto(Guid Id, DateTime StartedAt, GetUserDto User, GetStreamOptionDto? StreamOption)
{
    public Guid Id = Id;
    public DateTime StartedAt = StartedAt;
    public GetUserDto User = User;
    public GetStreamOptionDto? StreamOption = StreamOption;
}