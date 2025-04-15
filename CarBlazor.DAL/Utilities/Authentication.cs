using System.Security.Cryptography;
using System.Text;

namespace CarBlazor.DAL.Utilities;

public class Authentication
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);
            
        var hash = ComputeHash(password, salt);
            
        return (hash, salt);
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var computedHash = ComputeHash(password, storedSalt);
        return computedHash == storedHash;
    }

    private static string ComputeHash(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }
}