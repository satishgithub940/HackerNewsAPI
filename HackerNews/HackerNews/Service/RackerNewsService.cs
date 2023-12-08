using HackerNews.Model;
using HackerNews.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace HackerNews.Service
{
    public class RackerNewsService : IRackerNews
    {
        public static IConfiguration _configuration;
        public static IRackerNewsRepository _iRackerNewsRepository;
        private  readonly ILogger _logger;

        public RackerNewsService(IRackerNewsRepository iRackerNewsRepository, IConfiguration configuration, ILogger logger)
        {
            _iRackerNewsRepository = iRackerNewsRepository;
            _configuration = configuration;
            _logger = logger;
        }
        public List<NewsDbo> GetHackerNews()
        {
            try
            {
                var data = _iRackerNewsRepository.GetNewsCacheData();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    var newHackerData = GetHackerNewsAPI();
                    var newHackerNewsdata = _iRackerNewsRepository.SetHackerNewsData(newHackerData);
                    return newHackerData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetHackerNews Service Error Exception : " + ex);
                throw;
            }
        }
        public List<NewsDbo> GetHackerNewsAPI()
        {
            var NewsList = new List<NewsDbo>();

            var Ids = CallNewsIds().Take(Convert.ToInt32(_configuration["ApiSettings:NumberOfRecords"]));
            foreach (var id in Ids)
            {
                var result = CallNewsById(Convert.ToInt32(id));
                NewsList.Add(result);
            }
            return NewsList;
        }
         string CallNewsIds()
        {
            string apiUrl = _configuration["ApiSettings:HackerApiUrl"];
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Make a GET request
                    HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;
                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        string Ids = content;
                        Console.WriteLine(Ids);
                        return Ids;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CallNewsIds API Service Error Exception : " + ex);
                    throw;
                }
            }
        }
         NewsDbo CallNewsById(long id)
        {
            string apiUrl = _configuration["ApiSettings:HackerItemApiUrl"] + id + ".json";
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Make a GET request
                    HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and print the content of the response
                        string content = response.Content.ReadAsStringAsync().Result;
                        NewsDbo news = JsonSerializer.Deserialize<NewsDbo>(content);
                        Console.WriteLine(news);
                        return news;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CallNewsIds API Service Error Exception : " + ex);
                    throw;
                }
            }
        }
    }
}
