namespace Nexora.Core.Common.Helpers
{
    public static class CacheHelper
    {
        public static string GenerateKey(string cacheKey, params object[] keyParams)
            => keyParams.Any(x => !string.IsNullOrEmpty(Convert.ToString(x)))
                ? $"{cacheKey}_{string.Join("_", keyParams.Where(x => !string.IsNullOrEmpty(Convert.ToString(x))))}"
                : cacheKey;
    }
}