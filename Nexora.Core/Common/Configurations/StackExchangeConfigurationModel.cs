namespace Nexora.Core.Common.Configurations
{
    public sealed class StackExchangeConfigurationModel
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public string? Password { get; set; }
        public bool AbortOnConnectFail { get; set; } = false;
        public bool Ssl { get; set; } = true;
        public int ConnectTimeout { get; set; } = 3000;
        public int SyncTimeout { get; set; } = 3000;
        public int AsyncTimeout { get; set; } = 3000;
    }
}