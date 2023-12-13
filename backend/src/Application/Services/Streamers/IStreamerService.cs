namespace Application.Services.Streamers;

public interface IStreamerService
{
    string GenerateStreamerKey(User user);
    string DecryptStreamerKey(string encryptedStreamerKey);
}