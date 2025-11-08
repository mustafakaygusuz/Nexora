namespace Nexora.Core.Common.Enumerations
{
    public enum RateLimiterType
    {
        /// <summary>
        /// Implementing a basic fixed window rate limiter without additional penalties
        /// </summary>
        FixedWindow = 1,

        /// <summary>
        /// Incorporates a cooldown period after exceeding the rate limit
        /// </summary>
        Cooldown = 2
    }
}
