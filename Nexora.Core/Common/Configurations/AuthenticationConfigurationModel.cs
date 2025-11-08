namespace Nexora.Core.Common.Configurations
{
    public sealed class AuthenticationConfigurationModel
    {
        public string AccessTokenSecretKey { get; set; } = "";
        public string RefreshTokenSecretKey { get; set; } = "";
        public int AccessTokenExpireMinutes { get; set; }
        public int RefreshTokenExpireMinutes { get; set; }
    }
}