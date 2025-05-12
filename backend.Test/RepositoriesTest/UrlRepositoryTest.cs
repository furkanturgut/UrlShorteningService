using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Test.RepositoriesTest
{
    public class UrlRepositoryTest
    {
        private ApplicationDataContext CreateInMemoryDbContext ()
        {
            var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDataContext(options);
            context.Database.EnsureCreated();
            context.urls.AddRange
                (
                    new Url { Id=1,   LongUrl= "https://www.notion.so/al-ma-Notlar-15d5afef351e8027a91cce5a22f042d6#1eb5afef351e80afb3a5dcd08dac9a54", ShortUrl="9Abc" },
                    new Url { Id =2 , LongUrl = "https://www.notion.so/al-ma-Notlar-15d5afef351e8027a91cce5a22f042d6#1eb5afef351e80afb3a5dcd08dac9a54", ShortUrl = "asdc" },
                    new Url { Id =3 , LongUrl = "https://www.notion.so/al-ma-Notlar-15d5afef351e8027a91cce5a22f042d6#1eb5afef351e80afb3a5dcd08dac9a54", ShortUrl = "6y5m" }
                );
            context.SaveChanges();
            return context;
        }
        [Fact]
        public async Task UrlRepository_Create_ReturnUrl ()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var repository = new UrlRepository(datacontext);
            var url = new Url { Id = 4, LongUrl = "uzunurl.co", ShortUrl = "8Y6D" };
            // arrange is completed with the Url object above

            // act
            var result = await repository.Create(url);

            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(url);
            datacontext.urls.Count().Should().Be(4);
            datacontext.urls.Should().Contain(url);
        }

        [Fact]
        public async Task UrlRepository_Delete_ReturnUrl ()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var url = datacontext.urls.Where(url=> url.Id==1).FirstOrDefault();
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.Delete(url);
            //assert
            result.Should().BeEquivalentTo(url);
            result.Should().NotBeNull();
            datacontext.urls.Should().NotContain(url);
        }
        [Fact]
        public void UrlRepository_Delete_ReturnError ()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var url = new Url{Id=6, LongUrl="dbebebebeb.com", ShortUrl="haso"};
            var repository = new UrlRepository(datacontext); 
            //act
            var result = repository.Delete(url);
            //assert
            result.Should().NotBeEquivalentTo(url);
            result.Should().NotBeNull();
            datacontext.urls.Should().HaveCountGreaterThanOrEqualTo(3);
        }
       
       [Fact]
       public async Task UrlRepository_GetAll_ReturnsList ()
       {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var repository = new UrlRepository(datacontext);
            //act
            var result= await repository.GetAll();

            //assert
            result.Should().BeOfType<List<Url>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
       }
       [Fact]
       public async Task UrlRepository_GetById_ReturnUrl()
       {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var id = 1;
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetById(id);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Url>();
       }    
        [Fact]
        public async Task UrlRepository_GetById_ReturnNull ()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var id = 5;
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetById(id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UrlRepository_GetByLong_ReturnUrl()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var longUrl ="https://www.notion.so/al-ma-Notlar-15d5afef351e8027a91cce5a22f042d6#1eb5afef351e80afb3a5dcd08dac9a54";
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetByLong(longUrl);
            //assert
            result.Should().BeOfType<Url>();
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task UrlRepository_GetByLong_ReturnNullAsync()
        {
             //arrange
            var datacontext = CreateInMemoryDbContext();
            var longUrl = "denemememem.com";
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetByLong(longUrl);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UrlRepository_GetByShort_ReturnUrl()
        {
            //arrange
            var datacontext = CreateInMemoryDbContext();
            var longUrl ="9Abc";
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetByShort(longUrl);
            //assert
            result.Should().BeOfType<Url>();
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task UrlRepository_GetByShort_ReturnNull()
        {
             //arrange
            var datacontext = CreateInMemoryDbContext();
            var shortUrl = "denemememem.com";
            var repository = new UrlRepository(datacontext);
            //act
            var result = await repository.GetByShort(shortUrl);
            //assert
            result.Should().BeNull();
        }

    }
}
