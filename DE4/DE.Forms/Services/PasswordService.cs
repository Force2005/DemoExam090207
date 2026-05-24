using System.Security.Cryptography;

namespace DE.Forms.Services;

public sealed class PasswordService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;
    private const char Separator = '$';

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = deriveBytes.GetBytes(HashSize);

        return string.Join(
            Separator,
            "PBKDF2",
            Iterations.ToString(),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string password, string storedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedPassword))
        {
            return false;
        }

        var parts = storedPassword.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 4 && parts[0].Equals("PBKDF2", StringComparison.OrdinalIgnoreCase))
        {
            return VerifyPbkdf2Password(password, parts);
        }

        return string.Equals(password, storedPassword, StringComparison.Ordinal);
    }

    private static bool VerifyPbkdf2Password(string password, IReadOnlyList<string> parts)
    {
        try
        {
            if (!int.TryParse(parts[1], out var iterations))
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var actualHash = deriveBytes.GetBytes(expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
