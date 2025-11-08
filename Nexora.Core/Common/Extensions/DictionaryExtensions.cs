namespace Nexora.Core.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> MergeFrom(this Dictionary<string, string>? target, Dictionary<string, string>? source)
        {
            target ??= new Dictionary<string, string>();

            if (!source.HasValue())
            {
                return target;
            }

            foreach (var kvp in source!)
            {
                target[kvp.Key] = kvp.Value;
            }

            return target;
        }
    }
}