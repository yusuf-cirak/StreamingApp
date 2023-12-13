using Application.Abstractions.Helpers;

namespace Application.Services.Streamers;

public sealed class StreamerService : IStreamerService
{
    private readonly IEncryptionHelper _encryptionHelper;

    public StreamerService(IEncryptionHelper encryptionHelper)
    {
        _encryptionHelper = encryptionHelper;
    }

    public string GenerateStreamerKey(User user)
    {
        var plainStreamerKey = $"{user.Id}:{user.Username}";
        return _encryptionHelper.Encrypt(plainStreamerKey);
    }

    public string DecryptStreamerKey(string encryptedStreamerKey)
    {
        return _encryptionHelper.Decrypt(encryptedStreamerKey);
    }
}