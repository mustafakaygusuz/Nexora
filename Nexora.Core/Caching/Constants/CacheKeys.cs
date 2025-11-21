using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Caching.Constants
{
    public static class CacheKeys
    {
        #region Organizations
        //Example: v1_Organizations_GetBySubdomain_example
        //public static string OrganizationsGetBySubdomain(string subdomain) => $"v1_{CacheGroupType.Organizations}_GetBySubdomain_{subdomain}";
        #endregion

        #region SystemConfigurations
        public static string SystemConfigurationsList(string type) => $"v1_{CacheGroupType.SystemConfiguration}_List_{type}";
        #endregion

        #region StaticTexts
        public static string StaticTextsGetByKeyHashId(string type) => $"v1_StaticTexts_GetByKeyHash_{type}";

        public static string StaticTextsGetByKey(string languageId, string key) => $"v1_StaticTexts_GetByKey_{languageId}_{key}";

        public static string StaticTextsListByKeyHashId(string type) => $"v1_StaticTexts_ListByHash_{type}";

        public static string StaticTextsListByKey(string languageId) => $"v1_StaticTexts_ListByKey_{languageId}";
        #endregion
    }
}