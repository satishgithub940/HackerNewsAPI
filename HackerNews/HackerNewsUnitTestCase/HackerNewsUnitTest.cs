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
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public void GetHackerNews_ReturnsCachedData_WhenCacheIsNotEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IRackerNewsRepository>();
            var configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var cachedData = new List<NewsDbo> { /* initialize with some data */ };
            mockRepository.Setup(repo => repo.GetNewsCacheData()).Returns(cachedData);
            var mockRackerNews = new Mock<IRackerNewsRepository>();
            var service = new RackerNewsService(mockRackerNews.Object, (Microsoft.Extensions.Configuration.IConfiguration)configuration.Object);
            // Act
            var result = service.GetHackerNews();

            // Assert
            Assert.Same(cachedData, result);
            mockRepository.Verify(repo => repo.SetHackerNewsData(It.IsAny<List<NewsDbo>>()), Times.Never);
        }

        [Fact]
        public void GetHackerNews_ReturnsAPIData_WhenCacheIsEmpty()
        {
            // Arrange


            var mockRepository = new Mock<IRackerNewsRepository>();
            var configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var mockRackerNews = new Mock<IRackerNewsRepository>();

            var apiData = new List<NewsDbo> { /* initialize with some data */ };
            mockRepository.Setup(repo => repo.GetNewsCacheData()).Returns((List<NewsDbo>)null);
            mockRepository.Setup(repo => repo.SetHackerNewsData(It.IsAny<List<NewsDbo>>())).Returns(apiData);

            var service = new RackerNewsService(mockRackerNews.Object, configuration.Object);

            // Act
            var result = service.GetHackerNews();

            // Assert
            Assert.Same(apiData, result);
            mockRepository.Verify(repo => repo.SetHackerNewsData(apiData), Times.Once);
        }

        [Fact]
        public void GetHackerNews_ThrowsException_WhenExceptionOccurs()
        {
            // Arrange
            var mockRepository = new Mock<IRackerNewsRepository>();
            var configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var mockRackerNews = new Mock<IRackerNewsRepository>();


            mockRepository.Setup(repo => repo.GetNewsCacheData()).Throws(new Exception("Simulated exception"));

            var service = new RackerNewsService(mockRackerNews.Object,configuration.Object);


            // Act & Assert
            Assert.Throws<Exception>(() => service.GetHackerNews());
            mockRepository.Verify(repo => repo.SetHackerNewsData(It.IsAny<List<NewsDbo>>()), Times.Never);
        }
    }
}
