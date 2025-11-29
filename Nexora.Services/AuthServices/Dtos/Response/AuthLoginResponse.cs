namespace Nexora.Services.AuthServices.Dtos.Response
{
    public class AuthLoginResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpireMinutes { get; set; }
        public int ExpireSeconds { get; set; }
        public int RefreshExpireSeconds { get; set; }
    }
}