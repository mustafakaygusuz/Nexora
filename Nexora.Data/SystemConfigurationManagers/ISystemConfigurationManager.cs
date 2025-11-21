using Nexora.Data.Domain.Enumerations;

namespace Nexora.Data.SystemConfigurationManagers
{
    public interface ISystemConfigurationManager
    {
        /// <summary>
        /// Get organization system configurations by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<T> GetByType<T>(SystemConfigurationType type) where T : class, new();
    }
}