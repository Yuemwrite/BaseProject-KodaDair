using BCrypt.Net;

namespace BaseProject.Application.Utilities.Encryption;

public static class PasswordToolkit
{
    public static string EnhancedHashPassword(string plainText)
    {
        if (String.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));

        return BCrypt.Net.BCrypt.EnhancedHashPassword(plainText, hashType: HashType.SHA384);
    }

    public static bool EnhancedVerify(string requestPassword, string currentHashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(requestPassword, currentHashedPassword, hashType: HashType.SHA384);
    }
}