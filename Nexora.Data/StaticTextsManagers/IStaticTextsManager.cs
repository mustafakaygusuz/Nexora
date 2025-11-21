namespace Nexora.Data.StaticTextsManagers
{
    public interface IStaticTextsManager
    {
        /// <summary>
        /// Get static text by key in cache or db
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //Task<string?> GetByKey(string key, StaticTextType type);

        /// <summary>
        /// Get static text by key type in cache or db
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //Task<string?> GetByKey(StaticTextKeyType keyType, StaticTextType type);

        /// <summary>
        /// Lists static texts by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //Task<List<StaticTextsListByTypeResult>> ListByType(StaticTextType type);

        /// <summary>
        /// List static texts by keys and type.
        /// </summary>
        /// <param name="keyTypes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //Task<Dictionary<string, string>?> ListByKeys(List<StaticTextKeyType> keyTypes, StaticTextType type);
    }
}