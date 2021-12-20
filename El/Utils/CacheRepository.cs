using El.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace El.Utils
{
    public class CacheRepository
    {
        private readonly IDistributedCache cache;

        public CacheRepository(IDistributedCache Cache)
        {
            cache = Cache;
        }

        public void Add<T>(string key, T t, DistributedCacheEntryOptions? opt = null)
        {
            // Check
            if (t == null) {
                return;
            }

            // Serialize and set
            if (opt != null) {
                cache.SetString(key, t.SerializeObject(), opt);
            } else {
                cache.SetString(key, t.SerializeObject());
            }
        }
        public async Task AddAsync<T>(string key, T t, DistributedCacheEntryOptions? opt = null)
        {
            // Check
            if (t == null) {
                return;
            }

            // Serialize and set
            if (opt != null) {
                await cache.SetStringAsync(key, t.SerializeObject(), opt);
            } else {
                await cache.SetStringAsync(key, t.SerializeObject());
            }
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

        public T? Get<T>(string key)
        {
            //locals
            T? res = default;

            // Read and check
            var objStr = cache.GetString(key);
            if (objStr == null) {
                return res;
            }

            // Get and de-serialize
            res = objStr.DeserializeObject<T>();
            return res;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            //locals
            T? res = default;

            // Read and check
            var objStr = await cache.GetStringAsync(key, ct);
            if (objStr == null) {
                return res;
            }

            // Get and de-serialize
            res = objStr.DeserializeObject<T>();
            return res;
        }
    }
}
