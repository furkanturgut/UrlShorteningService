using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;

        public UrlService(IUrlRepository urlRepository)
        {
            this._urlRepository = urlRepository;
        }

        /// <summary>
        /// Creates a new shortened URL
        /// </summary>
        /// <param name="urlDto">Data transfer object containing original URL and optional custom alias</param>
        /// <returns>Created URL object or null if creation fails</returns>
        public async Task<Url?> Create(CreateUrlDto urlDto)
        {
            try
            {
                // Check if a custom alias was provided and is not the default value
                if(urlDto.customAlias != null && urlDto.customAlias != "string")
                {
                    // Verify the custom alias is not already in use
                    if (_urlRepository.ShortExist(urlDto.customAlias))
                    {
                        throw new Exception("This Alias Already Exist");
                    }
                    // Create new URL with custom alias
                    var urlWithAlias = new Url {LongUrl = urlDto.originalUrl, ShortUrl = urlDto.customAlias};
                    return await _urlRepository.Create(urlWithAlias);
                }
                
                // Generate a random short URL
                var shortUrl = CreateShortService.CreateShort();
                // Keep generating until we get a unique short URL
                while (_urlRepository.ShortExist(shortUrl))
                {
                    shortUrl = CreateShortService.CreateShort();
                }
                // Create new URL with generated short code
                var url = new Url {LongUrl = urlDto.originalUrl, ShortUrl = shortUrl};
                return await _urlRepository.Create(url);
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex;
            }
        }

        /// <summary>
        /// Deletes a URL by its ID
        /// </summary>
        /// <param name="Id">The ID of the URL to delete</param>
        /// <returns>The deleted URL or null if not found</returns>
        public async Task<Url?> Delete(int Id)
        {
            try
            {
                // Get the URL by ID
                var url = await _urlRepository.GetById(Id);
                if (url == null)
                {
                    // Return null if URL with specified ID doesn't exist
                    return null;
                }
                // Delete the URL and return it
                return await _urlRepository.Delete(url);
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex;
            }
        }

        /// <summary>
        /// Gets all URLs in the system
        /// </summary>
        /// <returns>Collection of URL DTOs or null if none exist</returns>
        public async Task<IEnumerable<ReturnUrlDto>?> GetAll()
        {
            try
            {
                // Retrieve all URLs from repository
                var urls = await _urlRepository.GetAll();
                if (urls == null)
                {
                    // Return null if no URLs exist
                    return null;
                }
                
                // Map URL entities to DTOs for client response
                var returnUrls = urls.Select(u => new ReturnUrlDto 
                { 
                    Id = u.Id,
                    LongUrl = u.LongUrl, 
                    shortUrl = u.ShortUrl 
                });
                
                return returnUrls;
            }
            catch (Exception ex)
            {
                // Rethrow the exception without explicitly mentioning ex
                throw;
            }
        }

        /// <summary>
        /// Gets a URL by its long (original) URL
        /// </summary>
        /// <param name="longUrl">The original URL to look up</param>
        /// <returns>URL object or null if not found</returns>
        public async Task<Url?> GetByLong(string longUrl)
        {
            try 
            {
                // Find URL by its original address
                return await _urlRepository.GetByLong(longUrl);
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex;
            }
        }

        /// <summary>
        /// Gets a URL by its shortened code
        /// </summary>
        /// <param name="shortUrl">The shortened URL code to look up</param>
        /// <returns>URL object or null if not found</returns>
        public async Task<Url?> GetByShort(string shortUrl)
        {
            try
            {
                // Find URL by its short code
                return await _urlRepository.GetByShort(shortUrl);
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex;
            }
        }
    }
}