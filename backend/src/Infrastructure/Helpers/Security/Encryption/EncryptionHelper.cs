using System.Text;
using Application.Abstractions.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Helpers.Security.Encryption;

public sealed class EncryptionHelper : IEncryptionHelper
{
    private readonly byte[] _key;

    public EncryptionHelper(IConfiguration configuration)
    {
        _key = Encoding.UTF8.GetBytes(configuration.GetSection("EncryptionKey").Get<string>()!);
    }
    
    public string Encrypt(string plainText)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = new byte[plaintextBytes.Length];

        for (int i = 0; i < plaintextBytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(plaintextBytes[i] ^ _key[i % _key.Length]);
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string cipherText)
    {
        byte[] encryptedBytes = Convert.FromBase64String(cipherText);
        byte[] decryptedBytes = new byte[encryptedBytes.Length];

        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            decryptedBytes[i] = (byte)(encryptedBytes[i] ^ _key[i % _key.Length]);
        }

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}