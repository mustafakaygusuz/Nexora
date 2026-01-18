using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Caching.Constants
{
    public static class CacheKeys
    {
        #region SystemConfigurations
        public static string SystemConfigurationsList(string type) => $"v1_{CacheGroupType.SystemConfiguration}_List_{type}";
        #endregion

        #region Consumers
        public static string ConsumersDataHashId(long consumerId) => $"v1_{CacheGroupType.Consumers}_Data_{consumerId}";

        public const string ConsumersDetail = $"v1_{nameof(CacheGroupType.Consumers)}_Detail";
        #endregion

        #region StaticTexts
        public static string StaticTextsGetByKeyHashId(string type) => $"v1_StaticTexts_GetByKeyHash_{type}";

        public static string StaticTextsGetByKey(string languageId, string key) => $"v1_StaticTexts_GetByKey_{languageId}_{key}";

        public static string StaticTextsListByKeyHashId(string type) => $"v1_StaticTexts_ListByHash_{type}";

        public static string StaticTextsListByKey(string languageId) => $"v1_StaticTexts_ListByKey_{languageId}";
        #endregion
    }
}