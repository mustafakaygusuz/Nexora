namespace Nexora.Core.Common.Helpers
{
    public static class EndpointHelper
    {
        public static string ModifyEndpoint(string endpoint, string apiType)
        {
            var versionMatch = System.Text.RegularExpressions.Regex.Match(endpoint, @"/v\d+/");
            if (versionMatch.Success)
            {
                var version = versionMatch.Value;
                var parts = endpoint.Split(new[] { version }, 2, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    return $"{parts[0]}{version}{apiType}/{parts[1]}";
                }
            }
            return endpoint;
        }

        public static string ModifyApiType(string apiType)
        {
            var apiTypeParts = apiType.Split('.');
            if (apiTypeParts.Length < 2)
            {
                return apiType;
            }

            return apiType.Split('.')[1].ToLower();
        }
    }
}