using HackerNews.Model;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public RackerNewsRepository(IMemoryCache memoryCache, ILogger logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public List<NewsDbo> GetNewsCacheData()
        {
            try
            {
                // Check if data is already in the cache
                List<NewsDbo> cachedData = _memoryCache.Get<List<NewsDbo>>("HackerNews");
                if (cachedData != null)
                {
                    return cachedData;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetNewsCacheData Repository Error Exception : " + ex);
                throw;
            }
        }

        // If data is not in the cache, fetch it and store in the cache
        public List<NewsDbo> SetHackerNewsData(List<NewsDbo> newHackerData)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("SetHackerNewsData Repository Error Exception : " + ex);
                throw;

            }
        }


    }
}
