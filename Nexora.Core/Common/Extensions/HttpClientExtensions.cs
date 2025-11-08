using System.Collections;
using System.Text.Json;

namespace Nexora.Core.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> GetResponseModel<TResult>(this HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            return jsonSerializerOptions == null ?
                responseContent.FromJson<TResult>() :
                responseContent.FromJson<TResult>(jsonSerializerOptions);
        }

        public static async Task<string> GetResponseContent(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public static string AppendQueryParameters<T>(this string url, T request) where T : class
        {
            var queryString = CreateQueryString(request.ToDictionary());

            return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
        }

        public static string AppendQueryParameters(this string url, List<KeyValuePair<string, string>> queryStringData)
        {
            var queryString = CreateQueryString(queryStringData);

            return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
        }

        private static string CreateQueryString(IDictionary<string, object> queryStringData)
        {
            var queryParameters = new List<string>();

            foreach (var queryString in queryStringData)
            {
                if (queryString.Value is IEnumerable enumerable && queryString.Value is not string)
                {
                    foreach (var item in enumerable)
                    {
                        queryParameters.Add($"{Uri.EscapeDataString(queryString.Key)}={Uri.EscapeDataString(item.ToString())}");
                    }
                }
                else
                {
                    queryParameters.Add($"{Uri.EscapeDataString(queryString.Key)}={Uri.EscapeDataString(queryString.Value.ToString())}");
                }
            }

            return queryParameters.HasValue() ? string.Join("&", queryParameters) : string.Empty;
        }

        private static string CreateQueryString(List<KeyValuePair<string, string>> queryStringData)
        {
            return queryStringData.HasValue() ?
                string.Join("&", queryStringData.Select(x =>
                    $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value.ToString())}").ToList()) :
                string.Empty;
        }
    }
}