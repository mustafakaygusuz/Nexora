using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Nexora.Core.StartupExtensions
{
    public class PrefixedKeyVaultSecretManager(IConfigurationManager configuration, string prefix = "") : KeyVaultSecretManager
    {
        private readonly string _prefix = !string.IsNullOrEmpty(prefix) ? $"{prefix}--" : "";

        public override bool Load(SecretProperties secret)
        {
            if (!secret.Name.StartsWith(_prefix))
            {
                return false;
            }

#if DEBUG
            if (configuration.GetValue<object>(secret.Name[_prefix.Length..].Replace("--", ConfigurationPath.KeyDelimiter)) != null && configuration.GetValue<string>(secret.Name[_prefix.Length..].Replace("--", ConfigurationPath.KeyDelimiter)) != "<set by key vault>")
            {
                return false;
            }
#endif

            return true;
        }

        public override string GetKey(KeyVaultSecret secret) => secret.Name[_prefix.Length..].Replace("--", ConfigurationPath.KeyDelimiter);
    }
}