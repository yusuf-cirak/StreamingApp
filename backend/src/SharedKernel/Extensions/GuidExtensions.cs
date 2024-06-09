using System.Security.Cryptography;
using System.Text;


namespace SharedKernel;

public static partial class GuidExtensions
{
    // Define a fixed namespace UUID
    private static readonly Guid FixedNamespace = Guid.Empty;

    public static Guid GenerateGuidFromString(string input)
    {
        // Convert the fixed namespace UUID to a byte array in big-endian order
        byte[] namespaceBytes = FixedNamespace.ToByteArray();
        SwapByteOrder(namespaceBytes);

        // Convert the input string to a byte array using UTF-8 encoding
        byte[] nameBytes = Encoding.UTF8.GetBytes(input);

        // Concatenate the namespace and name bytes
        byte[] hashInput = new byte[namespaceBytes.Length + nameBytes.Length];
        Buffer.BlockCopy(namespaceBytes, 0, hashInput, 0, namespaceBytes.Length);
        Buffer.BlockCopy(nameBytes, 0, hashInput, namespaceBytes.Length, nameBytes.Length);

        // Compute the SHA-1 hash
        byte[] hash;
        using (SHA1 sha1 = SHA1.Create())
        {
            hash = sha1.ComputeHash(hashInput);
        }

        // Set the version number to 5 (SHA-1)
        hash[6] = (byte)((hash[6] & 0x0F) | 0x50);
        // Set the variant to RFC 4122
        hash[8] = (byte)((hash[8] & 0x3F) | 0x80);

        // Convert hash to Guid
        byte[] newGuid = new byte[16];
        Array.Copy(hash, 0, newGuid, 0, 16);

        // Return the resulting Guid
        SwapByteOrder(newGuid);
        return new Guid(newGuid);
    }

    private static void SwapByteOrder(byte[] guid)
    {
        Swap(guid, 0, 3);
        Swap(guid, 1, 2);
        Swap(guid, 4, 5);
        Swap(guid, 6, 7);
    }

    private static void Swap(byte[] guid, int left, int right)
    {
        (guid[left], guid[right]) = (guid[right], guid[left]);
    }
}