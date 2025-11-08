namespace Nexora.Core.Common.Models
{
    public class ExternalServiceResultModel
    {
        public bool IsSuccess { get; set; } = true;
        public int HttpStatusCode { get; set; }
    }

    public sealed class ExternalServiceResultModel<TResult, TError> : ExternalServiceResultModel
        where TResult : class
    {
        public TResult? Result { get; set; }
        public TError? Error { get; set; }
    }

    public sealed class ExternalServiceResultModel<TError> : ExternalServiceResultModel
    {
        public TError? Error { get; set; }
    }
}