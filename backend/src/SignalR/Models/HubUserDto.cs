using System.Security.Claims;

namespace SignalR.Models;

public sealed record HubUserDto
{
    public string Id { get; }
    public string Username { get; }
    public string ProfileImageUrl { get; }

    private HubUserDto()
    {
    }

    public HubUserDto(string id, string username, string profileImageUrl)
    {
        Id = id;
        Username = username;
        ProfileImageUrl = profileImageUrl;
    }

    public static HubUserDto Create(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        var userName = user.FindFirstValue(ClaimTypes.Name) ?? "";
        var profileImageUrl = user.FindFirstValue("ProfileImageUrl") ?? "";

        return new HubUserDto(userId, userName, profileImageUrl);
    }
}