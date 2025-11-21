using Nexora.Core.Caching.Services;
using Nexora.Core.Contexts;
using Nexora.Data.Domain.DbContexts;

namespace Nexora.Data.StaticTextsManagers
{
    public sealed class StaticTextsManager(
        ApplicationDbContext _dbContext,
        ApiContext _apiContext,
        ICacheService _cacheService) : IStaticTextsManager
    {
        /// <summary>
        /// Get static text by key in cache or db
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public async Task<string?> GetByKey(string key, StaticTextType type)
        //{
        //    return await _cacheService.GetAndSetHash(
        //        CacheKeys.StaticTextsGetByKeyHashId(type.ToString()),
        //        CacheKeys.StaticTextsGetByKey(_apiContext.LanguageId, key), async () =>
        //        await _dbContext.StaticTexts
        //            .AsNoTracking()
        //            .Where(x =>
        //                x.Type == type &&
        //                x.Key == key)
        //            .Select(x => x.ValueNavigation.TranslationValues.FirstOrDefault(x => x.LanguageId == _apiContext.LanguageId)!.Value ?? "")
        //            .FirstOrDefaultAsync(),
        //            (DateTime.UtcNow.AddDays(1) - DateTime.UtcNow).TotalSeconds.ToInt());
        //}

        /// <summary>
        /// Get static text by key type in cache or db
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public async Task<string?> GetByKey(StaticTextKeyType keyType, StaticTextType type)
        //{
        //    var key = StaticTextKeyHelper.Get(keyType);

        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        return await GetByKey(key, type);
        //    }

        //    return null;
        //}

        /// <summary>
        /// Lists static texts by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public async Task<List<StaticTextsListByTypeResult>> ListByType(StaticTextType type)
        //{
        //    return await _cacheService.GetAndSetHash(
        //        CacheKeys.StaticTextsListByKeyHashId(type.ToString()),
        //        CacheKeys.StaticTextsListByKey(_apiContext.LanguageId),
        //        async () =>
        //        {
        //            return await _dbContext.StaticTexts
        //                .Where(x =>
        //                    x.Type == type)
        //                .Select(x => new StaticTextsListByTypeResult
        //                {
        //                    Key = x.Key,
        //                    Value = x.ValueNavigation.TranslationValues.First(y => y.LanguageId == _apiContext.LanguageId).Value ?? "",
        //                })
        //                .ToListAsync();
        //        },
        //        (_apiContext.CurrentDate.AddDays(1) - _apiContext.CurrentDate).TotalSeconds.ToInt());
    }

    /// <summary>
    /// List static texts by keys and type.
    /// </summary>
    /// <param name="keyTypes"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    //public async Task<Dictionary<string, string>?> ListByKeys(List<StaticTextKeyType> keyTypes, StaticTextType type)
    //{
    //    var keys = StaticTextKeyHelper.List(keyTypes);

    //    if (keys.HasValue())
    //    {
    //        var staticTexts = await ListByType(type);

    //        if (staticTexts.HasValue())
    //        {
    //            Dictionary<string, string> result = [];

    //            foreach (var item in keys)
    //            {
    //                if (staticTexts.Any(x => x.Key == item.Value))
    //                {
    //                    result.Add(item.Value, staticTexts.First(x => x.Key == item.Value).Value ?? "");
    //                }
    //            }

    //            if (result.HasValue())
    //            {
    //                return result;
    //            }
    //        }
    //    }

    //    return null;
    //}
}
