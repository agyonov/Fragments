using Db;
using El.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace El.Nomens
{
    public class Nomens : DbClassRoot
    {
        private readonly CacheRepository CRep;
        private const double cacheTimeoutMinutes = 45;

        public Nomens(BloggingContext db, CacheRepository cRep) : base(db)
        {
            CRep = cRep;
        }

        #region get / remove cache data generic methods

        /// <summary>
        /// This will be used to replace all the code duplication when getting cache
        /// </summary>
        /// <typeparam name="T">Type of the DB Entity</typeparam>
        /// <param name="cacheKey">Cache key </param>
        /// <param name="predicate">Optional query to filter results, for example x=>x.IsActive</param>
        /// <param name="lang">Optional language taken from session</param>
        /// <param name="useCache">If cached values should be used or instead the DB should be queried</param>
        /// <returns></returns>
        protected internal IEnumerable<T> GetCache<T>(string cacheKey, string lang = Const.Globalization.DefaultLang, Expression<Func<T, bool>>? predicate = null, bool useCache = true, IQueryable<T>? entitiesQ = null)
            where T : class
        {
            return GetCache<T, int>(cacheKey: cacheKey,
                           lang: lang,
                           predicate: predicate,
                           useCache: useCache,
                           entitiesQ: entitiesQ,
                           orderBy: null
                           );
        }

        protected internal async Task<IEnumerable<T>> GetCacheAsync<T>(string cacheKey, string lang = Const.Globalization.DefaultLang, Expression<Func<T, bool>>? predicate = null, bool useCache = true, IQueryable<T>? entitiesQ = null)
          where T : class
        {
            return await GetCacheAsync<T, int>(cacheKey: cacheKey,
                          lang: lang,
                          predicate: predicate,
                          useCache: useCache,
                          entitiesQ: entitiesQ,
                          orderBy: null
                          ).ConfigureAwait(false);
        }

        /// <summary>
        /// This will be used to replace all the code duplication when getting cache
        /// </summary>
        /// <typeparam name="T">Type of the DB Entity</typeparam>
        /// <typeparam name="TOrderParam">Type of the Order param</typeparam>
        /// <param name="cacheKey">Cache key </param>
        /// <param name="predicate">Optional query to filter results, for example x=>x.IsActive</param>
        /// <param name="lang">Optional language taken from session
        /// (used only for cache, if there's a different way to pull localized data from DB, provide it with predicate,
        /// if lang is provided cacheKey = cacheKey_lang)</param>
        /// <param name="useCache">If cached values should be used or instead the DB should be queried</param>
        /// <returns></returns>
        protected internal IEnumerable<T> GetCache<T, TOrderParam>(string cacheKey, string lang = Const.Globalization.DefaultLang, Expression<Func<T, bool>>? predicate = null, bool useCache = true, IQueryable<T>? entitiesQ = null, Expression<Func<T, TOrderParam>>? orderBy = null)
            where T : class
        {
            //locals
            IEnumerable<T>? entities = null;
            string cacheKeyExt = cacheKey + "_" + lang;
            //Check cache
            if (useCache) {
                entities = CRep.Get<IEnumerable<T>>(cacheKeyExt);
            }

            //Try to load data
            if (entities == null) {

                if (entitiesQ == null) {
                    entitiesQ = DB.Set<T>().AsNoTracking();
                }
                if (predicate != null) {
                    entitiesQ = entitiesQ.Where(predicate);
                }
                if (orderBy != null) {
                    entitiesQ = entitiesQ.OrderBy(orderBy);
                }
                entities = entitiesQ.ToList();

                //Try to add if cache to be used
                if (useCache) {
                    CRep.Add(cacheKeyExt, entities,
                        new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTimeoutMinutes)
                        });
                }
            }
            //return
            return entities;
        }
        protected internal async Task<IEnumerable<T>> GetCacheAsync<T, TOrderParam>(string cacheKey, string lang = Const.Globalization.DefaultLang, Expression<Func<T, bool>>? predicate = null, bool useCache = true, IQueryable<T>? entitiesQ = null, Expression<Func<T, TOrderParam>>? orderBy = null)
           where T : class
        {
            //locals
            IEnumerable<T>? entities = null;
            string cacheKeyExt = cacheKey + "_" + lang;
            //Check cache
            if (useCache) {
                entities = await CRep.GetAsync<IEnumerable<T>>(cacheKeyExt).ConfigureAwait(false);
            }

            //Try to load data
            if (entities == null) {

                if (entitiesQ == null) {
                    entitiesQ = DB.Set<T>().AsNoTracking();
                }
                if (predicate != null) {
                    entitiesQ = entitiesQ.Where(predicate);
                }
                if (orderBy != null) {
                    entitiesQ = entitiesQ.OrderBy(orderBy);
                }
                entities = await entitiesQ.ToListAsync().ConfigureAwait(false);

                //Try to add if cache to be used
                if (useCache) {
                    await CRep.AddAsync(cacheKeyExt, entities,
                        new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTimeoutMinutes)
                        }).ConfigureAwait(false);
                }
            }
            //return
            return entities;
        }

        public async Task RemoveCacheAsync(string cacheKey, string lang = Const.Globalization.DefaultLang)
        {
            string cacheKeyExt = cacheKey + "_" + lang;
            await CRep.RemoveAsync(cacheKeyExt);
        }

        public void RemoveCache(string cacheKey, string lang = Const.Globalization.DefaultLang)
        {
            string cacheKeyExt = cacheKey + "_" + lang;
            CRep.Remove(cacheKeyExt);
        }

        #endregion get / remove


        protected internal async Task<IEnumerable<Blog>> GetRStaticDataAsync(string? lang = null, bool useCache = true)
        {
            //Get current culture
            if (string.IsNullOrWhiteSpace(lang)) {
                lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }

            // return
            return await GetCacheAsync<Blog, int>(cacheKey: Const.CacheNames.Blog,
                                                        lang: lang,
                                                        useCache: useCache,
                                                        orderBy: x => x.Id);
        }
    }
}
