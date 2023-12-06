using HackerNews.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HackerNews.Repository
{
    public class RackerNewsRepository : IRackerNewsRepository
    {
        private readonly IMemoryCache _memoryCache;
        public RackerNewsRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<NewsDbo> GetNewsCacheData()
        {
            // Check if data is already in the cache
            List<NewsDbo> cachedData = _memoryCache.Get<List<NewsDbo>>("HackerNews");
            if (cachedData != null)
            {
                return cachedData;
            }
            return null;
        }

        // If data is not in the cache, fetch it and store in the cache
        public List<NewsDbo> SetHackerNewsData(List<NewsDbo> newHackerData)
        {
            List<NewsDbo> cachedData = _memoryCache.Get<List<NewsDbo>>("HackerNews");
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1000), // Cache for 1000 minutes
                SlidingExpiration = TimeSpan.FromMinutes(500) // Refresh the cache if not accessed for 500 minutes
            };

            _memoryCache.Set("HackerNews", newHackerData, cacheEntryOptions);

            return newHackerData;
        }


    }
}
