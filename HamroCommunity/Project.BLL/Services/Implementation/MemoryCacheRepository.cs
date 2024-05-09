using Microsoft.Extensions.Caching.Memory;
using Project.BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class MemoryCacheRepository : IMemoryCacheRepository
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            
        }
        public async Task<T?> GetCacheKey<T>(string cacheKey)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(cacheKey, out T? value) ? value: default(T));
        }

        public Task RemoveAsync(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            return Task.CompletedTask;
        }

        public async Task SetAsync<T>(string cacheKey, T value, MemoryCacheEntryOptions options, CancellationToken cancellationToken = default!)
        {
            if(value is not null)
            {
                _memoryCache.Set(cacheKey, value, options);
            }

            await Task.CompletedTask;
        }
    }
}
