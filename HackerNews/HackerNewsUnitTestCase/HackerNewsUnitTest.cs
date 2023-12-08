using HackerNews.Controllers;
using HackerNews.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace HackerNewsUnitTestCase
{
    public class HackerNewsUnitTest
    {
        [Fact] //Possitive case
        public void ReturnsOkObjectResult()
        {
            var mockRackerNews = new Mock<IRackerNews>();
            mockRackerNews.Setup(r => r.GetHackerNews());
            var controller = new HackerNewsController(mockRackerNews.Object);
            var result = controller.GetHackerNews();
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact] //Negative case
        public void ReturnsStatusCodeResult() 
        {
            var mockRackerNews = new Mock<IRackerNews>();
            mockRackerNews.Setup(r => r.GetHackerNews());
            var controller = new HackerNewsController(mockRackerNews.Object);
            var result = controller.GetHackerNews();
            // Assert
            Assert.IsType<StatusCodeResult>(result);
        }
    }
}
