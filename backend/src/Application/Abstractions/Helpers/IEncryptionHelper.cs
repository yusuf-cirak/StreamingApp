namespace Application.Abstractions.Helpers;

public interface IEncryptionHelper
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}