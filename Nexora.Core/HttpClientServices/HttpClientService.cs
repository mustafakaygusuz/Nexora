using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Common.JsonConverters;
using Nexora.Core.Common.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexora.Core.HttpClientServices
{
    public sealed class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpClientService> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger, IOptions<EncryptionConfigurationModel> options)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            _jsonSerializerOptions.Converters.Add(new EncryptIdConverterFactory(options.Value));
        }

        public async Task<ExternalServiceResultModel<TResult, TError>> Get<TResult, TError>(string url, List<KeyValuePair<string, string>>? queryStringData = null, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TResult : class
        {
            if (queryStringData.HasValue())
            {
                url = url.AppendQueryParameters(queryStringData!);
            }

            return await Send<TResult, TError>(url, HttpMethod.Get, headers: headers, useSSL: useSSL);
        }

        public async Task<ExternalServiceResultModel<TResult, TError>> Get<TResult, TRequest, TError>(string url, TRequest request, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TResult : class
            where TRequest : class
        {
            url = url.AppendQueryParameters(request);

            return await Send<TResult, TError>(url, HttpMethod.Get, headers: headers, useSSL: useSSL);
        }

        public async Task<ExternalServiceResultModel<TError>> Get<TError>(string url, List<KeyValuePair<string, string>>? queryStringData = null, Dictionary<string, string>? headers = null, bool useSSL = true)
        {
            if (queryStringData.HasValue())
            {
                url = url.AppendQueryParameters(queryStringData!);
            }

            return await Send<TError>(url, HttpMethod.Get, headers: headers, useSSL: useSSL);
        }

        public async Task<ExternalServiceResultModel<TError>> Get<TRequest, TError>(string url, TRequest request, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TRequest : class
        {
            url = url.AppendQueryParameters(request);

            return await Send<TError>(url, HttpMethod.Get, headers: headers, useSSL: useSSL);
        }

        public async Task<ExternalServiceResultModel<TResult, TError>> Post<TResult, TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TResult : class
            where TRequest : class
        {
            return await Send<TResult, TError>(url, HttpMethod.Post, request.ToJson(), headers, useSSL);
        }

        public async Task<ExternalServiceResultModel<TError>> Post<TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TRequest : class
        {
            return await Send<TError>(url, HttpMethod.Post, request.ToJson(), headers, useSSL);
        }

        public async Task<ExternalServiceResultModel<TResult, TError>> Put<TResult, TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TResult : class
            where TRequest : class
        {
            return await Send<TResult, TError>(url, HttpMethod.Put, request.ToJson(), headers, useSSL);
        }

        public async Task<ExternalServiceResultModel<TError>> Put<TRequest, TError>(TRequest request, string url, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TRequest : class
        {
            return await Send<TError>(url, HttpMethod.Put, request.ToJson(), headers, useSSL);
        }

        private async Task<ExternalServiceResultModel<TResult, TError>> Send<TResult, TError>(string url, HttpMethod method, string? content = null, Dictionary<string, string>? headers = null, bool useSSL = true)
            where TResult : class
        {
            ExternalServiceResultModel<TResult, TError> result = new ExternalServiceResultModel<TResult, TError>();

            try
            {
                var apiResult = await SendRequest(url, method, content, headers, useSSL);

                result.HttpStatusCode = apiResult.StatusCode.ToInt();

                if (apiResult.IsSuccessStatusCode)
                {
                    result.Result = await apiResult.GetResponseModel<TResult>(_jsonSerializerOptions);
                }
                else
                {
                    result.Error = await apiResult.GetResponseModel<TError>(_jsonSerializerOptions);
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.IsSuccess = false;
                result.HttpStatusCode = HttpStatusCode.InternalServerError.ToInt();
            }

            return result;
        }

        private async Task<ExternalServiceResultModel<TError>> Send<TError>(string url, HttpMethod method, string? content = null, Dictionary<string, string>? headers = null, bool useSSL = true)
        {
            ExternalServiceResultModel<TError> result = new ExternalServiceResultModel<TError>();

            try
            {
                var apiResult = await SendRequest(url, method, content, headers, useSSL);

                result.HttpStatusCode = apiResult.StatusCode.ToInt();

                if (!apiResult.IsSuccessStatusCode)
                {
                    result.Error = await apiResult.GetResponseModel<TError>(_jsonSerializerOptions);
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.IsSuccess = false;
                result.HttpStatusCode = HttpStatusCode.InternalServerError.ToInt();
            }

            return result;
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method, string? content = null, Dictionary<string, string>? headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(url),
                Content = !string.IsNullOrEmpty(content) ? new StringContent(content, Encoding.UTF8, "application/json") : null
            };

            if (headers.HasValue())
            {
                foreach (var headerItem in headers!)
                {
                    request.Headers.TryAddWithoutValidation(headerItem.Key, headerItem.Value);
                }
            }

            return request;
        }

        private async Task<HttpResponseMessage> SendRequest(string url, HttpMethod method, string? content = null, Dictionary<string, string>? headers = null, bool useSSL = true)
        {
            var client = _httpClientFactory.CreateClient(useSSL ? "SSL" : "NoSSL");

            return await client.SendAsync(CreateRequest(url, method, content, headers));
        }
    }
}