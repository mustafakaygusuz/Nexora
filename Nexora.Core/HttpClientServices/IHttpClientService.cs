using Nexora.Core.Common.Models;

namespace Nexora.Core.HttpClientServices
{
    public interface IHttpClientService
    {
        Task<ExternalServiceResultModel<TResult, TError>> Get<TResult, TError>(string url, List<KeyValuePair<string, string>>? queryStringData = null, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TResult : class;

        Task<ExternalServiceResultModel<TResult, TError>> Get<TResult, TRequest, TError>(string url, TRequest request, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TResult : class
           where TRequest : class;

        Task<ExternalServiceResultModel<TError>> Get<TError>(string url, List<KeyValuePair<string, string>>? queryStringData = null, Dictionary<string, string>? headers = null, bool useSSL = true);

        Task<ExternalServiceResultModel<TError>> Get<TRequest, TError>(string url, TRequest request, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TRequest : class;

        Task<ExternalServiceResultModel<TResult, TError>> Post<TResult, TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TResult : class
           where TRequest : class;

        Task<ExternalServiceResultModel<TError>> Post<TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TRequest : class;

        Task<ExternalServiceResultModel<TResult, TError>> Put<TResult, TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
           where TResult : class
           where TRequest : class;

        Task<ExternalServiceResultModel<TError>> Put<TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TRequest : class;
    }
}