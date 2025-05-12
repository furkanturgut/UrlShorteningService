using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend;
using backend.Dtos;
using backend.Interfaces;
using backend.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace backend.Test.ControllersTest
{
    public class UrlControllerTests
    {
       

        [Fact]
        public async Task CreateShort_WhenUrlDoesNotExist_CreatesAndReturnsNewUrl()
        {
            // Arrange
            var createDto = new CreateUrlDto { originalUrl = "https://example.com", customAlias = "custom" };
            var createdUrl = new Url { Id = 1, LongUrl = "https://example.com", ShortUrl = "custom" };
            var expectedResult = new ReturnUrlDto { LongUrl = "https://example.com", shortUrl = "custom" };

            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).Returns(Task.FromResult<Url?>(null));
            A.CallTo(() => urlService.Create(createDto)).Returns(Task.FromResult<Url?>(createdUrl));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.createShort(createDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedResult);
            
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).MustHaveHappenedOnceExactly();
            A.CallTo(() => urlService.Create(createDto)).MustHaveHappenedOnceExactly();
        }

      

        [Fact]
        public async Task CreateShort_WhenServiceCreateReturnsNull_Returns500()
        {
            // Arrange
            var createDto = new CreateUrlDto { originalUrl = "https://example.com" };
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).Returns(Task.FromResult<Url?>(null));
            A.CallTo(() => urlService.Create(createDto)).Returns(Task.FromResult<Url?>(null));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.createShort(createDto);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).MustHaveHappenedOnceExactly();
            A.CallTo(() => urlService.Create(createDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateShort_WhenServiceThrows_Returns500WithErrorMessage()
        {
            // Arrange
            var createDto = new CreateUrlDto { originalUrl = "https://example.com" };
            var exception = new Exception("Service error");
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).Throws(exception);

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.createShort(createDto);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("Service error");
            
            A.CallTo(() => urlService.GetByLong(createDto.originalUrl)).MustHaveHappenedOnceExactly();
        }

        

        [Fact]
        public async Task Redirect_WhenShortUrlExists_RedirectsToLongUrl()
        {
            // Arrange
            var shortUrl = "abc123";
            var url = new Url { Id = 1, LongUrl = "https://example.com", ShortUrl = shortUrl };
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetByShort(shortUrl)).Returns(Task.FromResult<Url?>(url));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.redirect(shortUrl);

            // Assert
            var redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
            redirectResult.Url.Should().Be(url.LongUrl);
            
            A.CallTo(() => urlService.GetByShort(shortUrl)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Redirect_WhenShortUrlDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var shortUrl = "notfound";
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetByShort(shortUrl)).Returns(Task.FromResult<Url?>(null));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.redirect(shortUrl);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            
            A.CallTo(() => urlService.GetByShort(shortUrl)).MustHaveHappenedOnceExactly();
        }

       

        [Fact]
        public async Task GetAll_WhenUrlsExist_ReturnsOkWithUrls()
        {
            // Arrange
            var urls = new List<ReturnUrlDto>
            {
                new ReturnUrlDto { Id = 1, LongUrl = "https://example1.com", shortUrl = "abc123" },
                new ReturnUrlDto { Id = 2, LongUrl = "https://example2.com", shortUrl = "def456" }
            };
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetAll()).Returns(Task.FromResult<IEnumerable<ReturnUrlDto>?>(urls));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(urls);
            
            A.CallTo(() => urlService.GetAll()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAll_WhenNoUrls_ReturnsNoContent()
        {
            // Arrange
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetAll()).Returns(Task.FromResult<IEnumerable<ReturnUrlDto>?>(null));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.GetAll();

            // Assert
            result.Should().BeOfType<NoContentResult>();
            
            A.CallTo(() => urlService.GetAll()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAll_WhenServiceThrows_Returns500WithErrorMessage()
        {
            // Arrange
            var exception = new Exception("Service error");
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.GetAll()).Throws(exception);

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.GetAll();

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("Service error");
            
            A.CallTo(() => urlService.GetAll()).MustHaveHappenedOnceExactly();
        }

  

        [Fact]
        public async Task Delete_WhenUrlExists_ReturnsDeletedUrl()
        {
            // Arrange
            var id = 1;
            var deletedUrl = new Url { Id = id, LongUrl = "https://example.com", ShortUrl = "abc123" };
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.Delete(id)).Returns(Task.FromResult<Url?>(deletedUrl));

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.Delete(id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(deletedUrl);
            
            A.CallTo(() => urlService.Delete(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Delete_WhenServiceThrows_Returns500WithErrorMessage()
        {
            // Arrange
            var id = 1;
            var exception = new Exception("Service error");
            
            var urlService = A.Fake<IUrlService>();
            A.CallTo(() => urlService.Delete(id)).Throws(exception);

            var controller = new UrlController(urlService);

            // Act
            var result = await controller.Delete(id);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("Service error");
            
            A.CallTo(() => urlService.Delete(id)).MustHaveHappenedOnceExactly();
        }

        
    }
}