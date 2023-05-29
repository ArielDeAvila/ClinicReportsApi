using BC = BCrypt.Net.BCrypt;

namespace ClinicReportsAPI.Tools;

public static class TokenGenerator
{
    public static string GenerateRandomToken(int length)
    {
        var salt = BC.GenerateSalt();
        var randomToken = BC.HashPassword(GetRandomString(length), salt);
        return randomToken;
    }

    public static string GetRandomString(int length)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var random = new Random();
        var token = new char[length];

        for (int i = 0; i < length; i++)
        {
            token[i] = allowedChars[random.Next(0, allowedChars.Length)];
        }

        return new string(token);
    }
}
