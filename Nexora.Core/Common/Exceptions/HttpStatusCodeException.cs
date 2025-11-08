using Newtonsoft.Json.Linq;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Helpers;
using System.Net;

namespace Nexora.Core.Common.Exceptions
{
    public sealed class HttpStatusCodeException : Exception
    {
        public string? Key { get; set; }
        public string? Title { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = "application/json";
        public List<StaticTextKeyType>? MessageKeyTypes { get; set; }

        public HttpStatusCodeException(HttpStatusCode statusCode) : base(string.Empty)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, StaticTextKeyType key, string message = "") : base(String.Format("Key:{0}, Message:{1}", key.ToString(), message))
        {
            Key = StaticTextKeyHelper.Get(key);
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, StaticTextKeyType key, List<StaticTextKeyType>? messageKeyTypes, string message = "") : base(String.Format("Message:{0}", message))
        {
            Key = StaticTextKeyHelper.Get(key);
            MessageKeyTypes = messageKeyTypes;
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string key, string message) : base(String.Format("Key:{0}, Message:{1}", key.ToString(), message))
        {
            Key = key;
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string key = "", string message = "", string title = "") : base(message)
        {
            Key = key;
            StatusCode = statusCode;
            Title = title;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        public HttpStatusCodeException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}