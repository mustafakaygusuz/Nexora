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
    }
}