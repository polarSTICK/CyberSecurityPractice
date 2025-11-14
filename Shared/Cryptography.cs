using System.Security.Cryptography;
using System.Text;

namespace Shared;

public class Cryptography
{
    public static byte[] GetHash(string? input)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(input ?? string.Empty));
    }

    public static string GetHashString(string? input)
    {
        StringBuilder sb = new();
        var hashBytes = GetHash(input);
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }
        
        return sb.ToString();
    }

    public static string GenerateSalt()
    {
        var randomGuid = Guid.NewGuid();
        return GetHashString(randomGuid.ToString());
    }
}