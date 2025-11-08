namespace Nexora.Core.Common.Configurations
{
    public sealed class CacheLockConfigurationModel
    {
        public List<ConfigurationModel>? CacheLockConfigurations { get; set; }

        public sealed class ConfigurationModel
        {
            public string Name { get; set; } = "";
            public int RepeatLockCount { get; set; }
            public int CounterSeconds { get; set; }
            public int LockSeconds { get; set; }
        }
    }
}