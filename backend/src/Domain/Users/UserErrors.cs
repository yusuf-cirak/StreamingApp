namespace Domain.Users;

public static class UserErrors
{
    public static Error AlreadyExists(string username) =>
        new Error("User.AlreadyExists",$"The user '{username}' already exists.");

    public static Error NotFound(string username) =>
    new Error("Users.NotFound", $"The user with '{username}' was not found.");

    public static Error NotFound(Guid guid) =>
    new Error("Users.NotFound", $"The user with '{guid}' was not found.");
}
