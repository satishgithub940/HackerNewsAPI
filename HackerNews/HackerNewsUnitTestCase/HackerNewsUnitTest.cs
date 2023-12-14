using Castle.Core.Configuration;
using HackerNews.Controllers;
using HackerNews.Model;
using HackerNews.Repository;
using HackerNews.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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

            Assert.NotNull(result);
        }


        [Fact]
        public void GetHackerNewsReturnsAPIData()
        {
            // Arrange
            var mockRepository = new Mock<IRackerNewsRepository>();
            var apiData = new List<NewsDbo>
                {
                    new NewsDbo
                    {
                        score = 5,
                        title = "SunRocket cofounders up and leave",
                        type = "story",
                        url = "http://www.washingtonpost.com/wp-dyn/content/article/2007/02/11/AR2007021101198.html?nav=rss_technology"
                    },
                    new NewsDbo
                    {
                        score = 3,
                        title = "How Much Money Do You Make?",
                        type = "story",
                        url = "http://www.techcrunch.com/2006/10/12/how-much-money-do-you-make/"
                    }
                };

            mockRepository.Setup(repo => repo.GetNewsCacheData()).Returns((List<NewsDbo>)apiData);
            mockRepository.Setup(repo => repo.SetHackerNewsData(It.IsAny<List<NewsDbo>>())).Returns(apiData);

            var service = new RackerNewsService(mockRepository.Object, null);

            // Act
            var result = service.GetHackerNews();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(apiData, result);
        }

        [Fact]
        public void GetHackerNewsReturnsDataType()
        {
            // Arrange
            var mockRepository = new Mock<IRackerNewsRepository>();
            var apiData = new List<NewsDbo>
                {
                    new NewsDbo
                    {
                        score = 5,
                        title = "SunRocket cofounders up and leave",
                        type = "story",
                        url = "http://www.washingtonpost.com/wp-dyn/content/article/2007/02/11/AR2007021101198.html?nav=rss_technology"
                    },
                    new NewsDbo
                    {
                        score = 3,
                        title = "How Much Money Do You Make?",
                        type = "story",
                        url = "http://www.techcrunch.com/2006/10/12/how-much-money-do-you-make/"
                    }
                };

            mockRepository.Setup(repo => repo.GetNewsCacheData()).Returns((List<NewsDbo>)apiData);
            mockRepository.Setup(repo => repo.SetHackerNewsData(It.IsAny<List<NewsDbo>>())).Returns(apiData);
            var service = new RackerNewsService(mockRepository.Object, null);
            // Act
            var result = service.GetHackerNews();

           
            // Assert
            Assert.IsType<List<NewsDbo>>(result);
        }
    }
}
