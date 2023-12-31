namespace Application.Features.Users.Abstractions;

public interface IUserCommandRequest
{
    public Guid UserId { get; init; }
}