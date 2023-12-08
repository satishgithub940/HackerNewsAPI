using HackerNews.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HackerNews.Controllers
{

    /// <summary>
    /// Represents a controller for getting hacker news data.
    /// </summary>
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        public static IRackerNews _iRackerNews;
        public HackerNewsController(IRackerNews iRackerNews)
        {
            _iRackerNews = iRackerNews;
        }


        /// <summary>
        /// Gets a list of top 200 Hacker News 
        /// </summary>
        [HttpGet]
        [Route("api/GetHackerNews")]
        public ActionResult GetHackerNews() 
        {
            var result = _iRackerNews.GetHackerNews();
            return Ok(result);
        }
    }
}
