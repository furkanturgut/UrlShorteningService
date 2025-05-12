using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Interfaces;
using backend.Models;
using backend.Services;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace backend.Test.ServicesTest
{
    public class UrlServiceTests
    {
       
        
        [Fact]
        public async Task GetByShort_WhenUrlExists_ReturnsUrl()
        {
            // Arrange
            var shortUrl = "abc123";
            var url = new Url { Id = 1, LongUrl = "https://example.com", ShortUrl = shortUrl };
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetByShort(shortUrl)).Returns(Task.FromResult<Url?>(url));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.GetByShort(shortUrl);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(url);
            A.CallTo(() => repository.GetByShort(shortUrl)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task GetByShort_WhenUrlDoesNotExist_ReturnsNull()
        {
            // Arrange
            var shortUrl = "notfound";
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetByShort(shortUrl)).Returns(Task.FromResult<Url?>(null));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.GetByShort(shortUrl);
            
            // Assert
            result.Should().BeNull();
            A.CallTo(() => repository.GetByShort(shortUrl)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task GetByShort_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var shortUrl = "exception";
            var repository = A.Fake<IUrlRepository>();
            var exception = new Exception("Repository error");
            
            A.CallTo(() => repository.GetByShort(shortUrl)).Throws(exception);
            
            var service = new UrlService(repository);
            
            // Act & Assert
            Func<Task> act = async () => await service.GetByShort(shortUrl);
            await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Repository error");
            
            A.CallTo(() => repository.GetByShort(shortUrl)).MustHaveHappenedOnceExactly();
        }
        
      
        
        [Fact]
        public async Task GetByLong_WhenUrlExists_ReturnsUrl()
        {
            // Arrange
            var longUrl = "https://example.com/very/long/path";
            var url = new Url { Id = 1, LongUrl = longUrl, ShortUrl = "abc123" };
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetByLong(longUrl)).Returns(Task.FromResult<Url?>(url));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.GetByLong(longUrl);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(url);
            A.CallTo(() => repository.GetByLong(longUrl)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task GetByLong_WhenUrlDoesNotExist_ReturnsNull()
        {
            // Arrange
            var longUrl = "https://notfound.com";
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetByLong(longUrl)).Returns(Task.FromResult<Url?>(null));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.GetByLong(longUrl);
            
            // Assert
            result.Should().BeNull();
            A.CallTo(() => repository.GetByLong(longUrl)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task GetByLong_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var longUrl = "https://exception.com";
            var repository = A.Fake<IUrlRepository>();
            var exception = new Exception("Repository error");
            
            A.CallTo(() => repository.GetByLong(longUrl)).Throws(exception);
            
            var service = new UrlService(repository);
            
            // Act & Assert
            Func<Task> act = async () => await service.GetByLong(longUrl);
            await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Repository error");
            
            A.CallTo(() => repository.GetByLong(longUrl)).MustHaveHappenedOnceExactly();
        }
        
       
        
        [Fact]
        public async Task Delete_WhenUrlExists_ReturnsDeletedUrl()
        {
            // Arrange
            var id = 1;
            var url = new Url { Id = id, LongUrl = "https://example.com", ShortUrl = "abc123" };
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetById(id)).Returns(Task.FromResult<Url?>(url));
            A.CallTo(() => repository.Delete(url)).Returns(Task.FromResult(url));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.Delete(id);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(url);
            A.CallTo(() => repository.GetById(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Delete(url)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task Delete_WhenUrlDoesNotExist_ReturnsNull()
        {
            // Arrange
            var id = 999;
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.GetById(id)).Returns(Task.FromResult<Url?>(null));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.Delete(id);
            
            // Assert
            result.Should().BeNull();
            A.CallTo(() => repository.GetById(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Delete(A<Url>._)).MustNotHaveHappened();
        }
        
        [Fact]
        public async Task Delete_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var id = 1;
            var url = new Url { Id = id, LongUrl = "https://example.com", ShortUrl = "abc123" };
            var repository = A.Fake<IUrlRepository>();
            var exception = new Exception("Repository error");
            
            A.CallTo(() => repository.GetById(id)).Returns(Task.FromResult<Url?>(url));
            A.CallTo(() => repository.Delete(url)).Throws(exception);
            
            var service = new UrlService(repository);
            
            // Act & Assert
            Func<Task> act = async () => await service.Delete(id);
            await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Repository error");
            
            A.CallTo(() => repository.GetById(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Delete(url)).MustHaveHappenedOnceExactly();
        }
        
       
        
        [Fact]
        public async Task Create_WithCustomAlias_WhenAliasDoesNotExist_ReturnsCreatedUrl()
        {
            // Arrange
            var dto = new CreateUrlDto 
            { 
                originalUrl = "https://example.com",
                customAlias = "custom" 
            };
            
            var createdUrl = new Url 
            { 
                Id = 1, 
                LongUrl = dto.originalUrl, 
                ShortUrl = dto.customAlias 
            };
            
            var repository = A.Fake<IUrlRepository>();
            
            A.CallTo(() => repository.ShortExist(dto.customAlias)).Returns(false);
            A.CallTo(() => repository.Create(A<Url>._)).Returns(Task.FromResult(createdUrl));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.Create(dto);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(createdUrl);
            A.CallTo(() => repository.ShortExist(dto.customAlias)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Create(A<Url>.That.Matches(u => 
                u.LongUrl == dto.originalUrl && 
                u.ShortUrl == dto.customAlias))).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task Create_WithCustomAlias_WhenAliasAlreadyExists_ThrowsException()
        {
            // Arrange
            var dto = new CreateUrlDto 
            { 
                originalUrl = "https://example.com",
                customAlias = "existing" 
            };
            
            var repository = A.Fake<IUrlRepository>();
            A.CallTo(() => repository.ShortExist(dto.customAlias)).Returns(true);
            
            var service = new UrlService(repository);
            
            // Act & Assert
            Func<Task> act = async () => await service.Create(dto);
            await act.Should().ThrowExactlyAsync<Exception>()
                .WithMessage("This Alias Already Exist");
            
            A.CallTo(() => repository.ShortExist(dto.customAlias)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Create(A<Url>._)).MustNotHaveHappened();
        }
        
        [Fact]
        public async Task Create_WithoutCustomAlias_GeneratesUniqueShortUrl()
        {
            // Arrange
            var dto = new CreateUrlDto 
            { 
                originalUrl = "https://example.com",
                customAlias = "string" // Default value that should be ignored
            };
            
            var createdUrl = new Url 
            { 
                Id = 1, 
                LongUrl = dto.originalUrl, 
                ShortUrl = "random123" // Won't actually match but that's fine for the test
            };
            
            var repository = A.Fake<IUrlRepository>();
            
            // First check returns true (exists), second check returns false (doesn't exist)
            A.CallTo(() => repository.ShortExist(A<string>._))
                .ReturnsNextFromSequence(true, false);
            
            A.CallTo(() => repository.Create(A<Url>._)).Returns(Task.FromResult(createdUrl));
            
            var service = new UrlService(repository);
            
            // Act
            var result = await service.Create(dto);
            
            // Assert
            result.Should().NotBeNull();
            result.LongUrl.Should().Be(dto.originalUrl);
            A.CallTo(() => repository.ShortExist(A<string>._)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => repository.Create(A<Url>.That.Matches(u => 
                u.LongUrl == dto.originalUrl))).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task Create_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var dto = new CreateUrlDto 
            { 
                originalUrl = "https://example.com",
                customAlias = "custom" 
            };
            
            var repository = A.Fake<IUrlRepository>();
            var exception = new Exception("Repository error");
            
            A.CallTo(() => repository.ShortExist(dto.customAlias)).Returns(false);
            A.CallTo(() => repository.Create(A<Url>._)).Throws(exception);
            
            var service = new UrlService(repository);
            
            // Act & Assert
            Func<Task> act = async () => await service.Create(dto);
            await act.Should().ThrowExactlyAsync<Exception>().WithMessage("Repository error");
            
            A.CallTo(() => repository.ShortExist(dto.customAlias)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.Create(A<Url>._)).MustHaveHappenedOnceExactly();
        }
        
        
    }
}