namespace Nexora.Core.Common.Configurations
{
    public sealed class EncryptionConfigurationModel
    {
        public List<ConfigurationModel>? EncryptionConfigurations { get; set; }

        public sealed class ConfigurationModel
        {
            public string Name { get; set; } = "";
            public string Key { get; set; } = "";
            public string IV { get; set; } = "";
        }
    }
}