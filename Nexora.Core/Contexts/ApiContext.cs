using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Constants;

namespace Nexora.Core.Contexts
{
    public class ApiContext
    {
        public string LanguageId { get; set; } = "tr";
        public long ConsumerId { get; set; }
        public SystemChannelType ChannelType { get; set; }
        public int TimeZone { get; set; } = 3;
        public string IpAddress { get; set; }
        public List<int> Roles { get; set; }
        public DateTime CurrentDate => DateTime.UtcNow.AddHours(TimeZone);
        public long? SystemUserId { get; set; }
        public long? UserId { get; set; }


        public void SetContext(Dictionary<string, string> headerData, string defaultLanguageId = "")
        {
            LanguageId = GetHeaderValue(headerData, HeaderConstants.LanguageId, out var languageId) ? !string.IsNullOrEmpty(languageId) ? languageId : defaultLanguageId : defaultLanguageId;
            ChannelType = GetHeaderValue(headerData, HeaderConstants.ChannelType, out var channelType) ? !string.IsNullOrEmpty(channelType) ? channelType.ParseEnum<SystemChannelType>() : default : default;
        }

        private static bool GetHeaderValue(Dictionary<string, string> headerData, string key, out string? headerValue)
        {
            headerValue = (headerData.TryGetValue(key, out var value) || headerData.TryGetValue(key.ToLower(), out value)) && !string.IsNullOrEmpty(value) ? value : null;

            return headerValue != null;
        }
    }
}