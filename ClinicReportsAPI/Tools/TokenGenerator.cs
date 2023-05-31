namespace ClinicReportsAPI.Tools;

public static class TokenGenerator
{
    public static string GenerateRandomToken()
    {
        string token = Guid.NewGuid().ToString("N");

        return token;
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
