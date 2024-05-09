using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IMemoryCacheRepository
    {
        Task<T?> GetCacheKey<T>(string cacheKey);
        Task SetAsync<T>(string cacheKey, T value, MemoryCacheEntryOptions options, CancellationToken cancellationToken);
        Task RemoveAsync(string cacheKey);
    }
}
