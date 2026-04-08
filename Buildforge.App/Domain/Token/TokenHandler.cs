using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;

namespace Buildforge.App.Domain.Token;

public sealed class TokenHandler
{
    private readonly byte[] Entropy = Encoding.UTF8.GetBytes(nameof(Buildforge));

    private string GetTokenFilePath()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(Buildforge));

        Directory.CreateDirectory(directory);

        return Path.Combine(directory, "buildforge.token.json");
    }

    public async Task SaveToken(Token token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(token));

        byte[] encryptedBytes = ProtectedData.Protect(bytes, Entropy, DataProtectionScope.CurrentUser);

        await File.WriteAllBytesAsync(GetTokenFilePath(), encryptedBytes);
    }

    public bool HasValidToken()
    {
        if (!TryLoadToken(out var token))
        {
            return false;
        }

        return token switch
        {
            Token.V1 v1 => v1.UtcExpiry <= DateTime.UtcNow.AddMinutes(-5),
            _ => false
        };
    }

    public bool TryLoadToken([NotNullWhen(returnValue: true)] out Token? token)
    {
        token = null;

        if (!File.Exists(GetTokenFilePath()))
        {
            return false;
        }

        byte[] encryptedBytes = File.ReadAllBytes(GetTokenFilePath());

        byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, Entropy, DataProtectionScope.CurrentUser);

        token = JsonSerializer.Deserialize<Token>(Encoding.UTF8.GetString(decryptedBytes));

        return token is not null;
    }
}