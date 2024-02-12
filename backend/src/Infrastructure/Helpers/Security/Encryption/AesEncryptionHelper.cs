using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Helpers.Security.Encryption;

public sealed class AesEncryptionHelper : IEncryptionHelper
{
    private readonly byte[] _key; // 16, 24, or 32 bytes
    private readonly byte[] _iv; // 16 bytes

    public AesEncryptionHelper(IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration.GetSection("EncryptionKey").Get<string>()!);
        _key = key;
        _iv = key;
    }

    public string Encrypt(string plainText)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            byte[] encrypted;

            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            return BitConverter.ToString(encrypted).Replace("-", "");
        }
    }

    public string Decrypt(string cipherText)
    {
        var cipherBytes = new byte[cipherText.Length / 2];
        for (int i = 0; i < cipherBytes.Length; i++)
        {
            cipherBytes[i] = Convert.ToByte(cipherText.Substring(i * 2, 2), 16);
        }

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            string plaintext;

            using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }
    }
}