using HackerNews.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        public static IRackerNews _iRackerNews;
        public HackerNewsController(IRackerNews iRackerNews)
        {
            _iRackerNews = iRackerNews;
        }

        [HttpGet]
        [Route("api/GetHackerNews")]
        public ActionResult Index() 
        {
            var result = _iRackerNews.GetHackerNews();
            return Ok(result);
        }
    }
}
