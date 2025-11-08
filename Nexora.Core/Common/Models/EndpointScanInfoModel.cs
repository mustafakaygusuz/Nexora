namespace Nexora.Core.Common.Models
{
    public sealed class EndpointScanInfoModel
    {
        public required string Url { get; set; }
        public required string HttpMethod { get; set; }
    }
}