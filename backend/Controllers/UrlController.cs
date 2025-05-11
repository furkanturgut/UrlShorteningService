using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend
{
   
    public class UrlController : Controller
    {
        private readonly IUrlService _service;

      
        public UrlController(IUrlService urlService)
        {
            this._service = urlService;
        }

        /// <summary>
        /// Creates a shortened URL from an original URL
        /// </summary>
        /// <param name="newUrlDto">DTO containing the original URL</param>
        /// <returns>The original and shortened URL</returns>
        [HttpPost]
        [Route("url/create")]
        public async Task<IActionResult> createShort ([FromBody] CreateUrlDto newUrlDto)
        {
            try
            {
                // Check if URL already exists in database
                if (await _service.GetByLong(newUrlDto.originalUrl) != null)
                {
                    // Return existing short URL if the original URL is already in the database
                    var existedUrl = await _service.GetByLong(newUrlDto.originalUrl);
                    return Ok(new ReturnUrlDto {LongUrl= existedUrl.LongUrl, shortUrl= existedUrl.ShortUrl});
                }
                
                // Create new short URL
                var url = await _service.Create(newUrlDto);
                if (url is null)
                {
                    // Throw exception if URL creation fails
                    throw new Exception();
                }
                
                // Return the newly created URL mapping
                return Ok(new ReturnUrlDto {LongUrl= url.LongUrl, shortUrl= url.ShortUrl});
            }
            catch (Exception ex)
            {
                // Return error status if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Redirects from a short URL to the original URL
        /// </summary>
        /// <param name="shortUrl">The shortened URL code</param>
        /// <returns>Redirect to original URL or NotFound</returns>
        [HttpGet]
        [Route("{shortUrl}")]
        public async Task<IActionResult> redirect(string shortUrl)
        {
            // Find URL object by short URL
            var urlObject = await _service.GetByShort(shortUrl);
            if (urlObject is null)
            {
                // Return 404 if short URL is not found
                return NotFound();
            }
            
            // Redirect to the original URL
            return Redirect(urlObject.LongUrl);
        }

        /// <summary>
        /// Gets all URLs in the database
        /// </summary>
        /// <returns>List of all URL mappings</returns>
        [HttpGet]
        [Route("url/all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var urls = await _service.GetAll();
                if (urls == null || !urls.Any())
                {
                    return NoContent();
                }
                return Ok(urls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a URL mapping by its short URL
        /// </summary>
        /// <param name="Id">The id of url</param>
        /// <returns>Success message or error status</returns>
        [HttpDelete]
        [Route("url/delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {                
                // Delete the URL
                var  url = await _service.Delete(Id);

                return Ok(url);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}