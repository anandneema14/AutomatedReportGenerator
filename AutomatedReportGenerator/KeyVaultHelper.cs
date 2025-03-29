using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AutomatedReportGenerator;

public static class KeyVaultHelper
{
    public static string GetConnectionString(string keyVaultName, string clientId, string clientSecret, string tenantId, string secretName)
    {
        try
        {
            string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";
            var client = new SecretClient(new Uri(keyVaultUri), new ClientSecretCredential(tenantId, clientId, clientSecret));
            KeyVaultSecret secret = client.GetSecret(secretName);
            return secret.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing Key Vault: {ex.Message}");
            return string.Empty;
        }
    }
}