namespace Nexora.Core.Common.Configurations
{
    public sealed class MailSmtpConfigurationModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Port { get; set; }
        public string? Server { get; set; }
    }
}