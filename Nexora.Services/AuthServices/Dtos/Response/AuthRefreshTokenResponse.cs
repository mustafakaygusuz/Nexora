namespace Nexora.Services.AuthServices.Dtos.Response
{
    public sealed class AuthRefreshTokenResponse
    {
        public string? AccessToken { get; set; }
        public int ExpireMinutes { get; set; }
    }
}