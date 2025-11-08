using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RateLimitAttribute(RateLimiterType type, int period, TimeUnitType periodTimeUnit, int limit, int? blockPeriodInSeconds = null, string? clientIdHeader = null, List<string>? clientWhitelist = null) : Attribute
    {
        /// <summary>
        /// Gets or sets the type of rate limiter.
        /// </summary>
        /// <value>A RateLimiter type value.</value>
        public RateLimiterType Type { get; } = type;

        /// <summary>Rate limit period.</summary>
        /// <value>An integer value with rate limit period.</value>
        public int Period { get; } = period;

        /// <summary>Rate limit period time unit as in seconds, minutes, hours etc.</summary>
        /// <value>A TimeUnit type value with rate limit period.</value>
        public TimeUnitType PeriodTimeUnit { get; } = periodTimeUnit;

        /// <summary>
        /// Maximum number of requests that a client can make in a defined period.
        /// </summary>
        /// <value>An integer value with maximum number of requests.</value>
        public int Limit { get; } = limit;

        /// <summary>
        /// Rate limit block period after reaching limit, in seconds.
        /// </summary>
        /// <value>An integer value with block period in seconds.</value>
        public int? BlockPeriodInSeconds { get; } = blockPeriodInSeconds;

        /// <summary>
        /// Gets or sets the HTTP header that holds the client identifier.
        /// </summary>
        /// <value>A string value with the HTTP header.</value>
        public string? ClientIdHeader { get; } = clientIdHeader;

        /// <summary>Gets the list of white listed clients.</summary>
        /// <value>
        /// A <see cref="T:System.Collections.Generic.List`1" /> collection with white listed clients.
        /// </value>
        public List<string> ClientWhitelist { get; } = clientWhitelist ?? [];

        /// <summary>
        /// Allowed ip list
        /// <see cref="T:System.Collections.Generic.List`1" />
        /// </summary>
        public List<string> AllowedIPList { get; set; } = [];

        /// <summary>
        /// Gets or sets the HTTP Status code returned when rate limiting occurs, by default value is set to 429 (Too Many Requests).
        /// </summary>
        /// <value>
        /// An integer value with the HTTP Status code.
        /// <para>Default value: 429 (Too Many Requests).</para>
        /// </value>
        public int HttpStatusCode { get; set; } = 429;

        /// <summary>
        /// Gets or sets a value that will be used as a formatter for the QuotaExceeded response message.
        /// <para>If none specified the default will be: "Too many requests. Please try again later.".</para>
        /// </summary>
        /// <value>
        /// A string value with a formatter for the QuotaExceeded response message.
        /// <para>Default will be: "Too many requests. Please try again later.".</para>
        /// </value>
        public string QuotaExceededMessage { get; set; } = "Too many requests. Please try again later.";
    }
}