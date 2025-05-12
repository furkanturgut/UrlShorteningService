using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDataContext _context;

   
        public UrlRepository(ApplicationDataContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a new URL in the database
        /// </summary>
        /// <param name="url">URL object to be saved</param>
        /// <returns>The created URL object or null if creation fails</returns>
        public async Task<Url?> Create(Url url)
        {
            try
            {
                // Add URL to database context
                await _context.AddAsync(url);
                // Save changes to database
                await _context.SaveChangesAsync();
                return url;
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex;
            }
              
        }

        /// <summary>
        /// Deletes a URL from the database
        /// </summary>
        /// <param name="url">URL object to be deleted</param>
        /// <returns>The deleted URL object or null if deletion fails</returns>
        public async Task<Url?> Delete(Url url)
        {
            try
            {
                // Remove URL from database context
                _context.Remove(url);
                // Save changes to database
                await _context.SaveChangesAsync();
                return url;
            }
            catch (Exception ex)
            {
                // Rethrow the exception for handling at a higher level
                throw ex; 
            }
        }

        /// <summary>
        /// Gets all URLs from the database
        /// </summary>
        /// <returns>List of all URL objects in the database</returns>
        public async Task<List<Url>?> GetAll()
        {
            // Return all URLs as a list
            return await _context.urls.ToListAsync();
        }

        /// <summary>
        /// Gets a URL by its ID
        /// </summary>
        /// <param name="Id">The ID of the URL to find</param>
        /// <returns>URL object with the specified ID or null if not found</returns>
        public async Task<Url?> GetById(int Id)
        {
            // Use FindAsync for efficient primary key lookup
            return await _context.urls.FindAsync(Id);
        }

        /// <summary>
        /// Gets a URL by its long (original) URL
        /// </summary>
        /// <param name="LongUrl">The original URL to find</param>
        /// <returns>URL object containing the long URL or null if not found</returns>
        public async Task<Url?> GetByLong(string LongUrl)
        {
            // Find URL where the long URL contains the provided string
            return await _context.urls.Where(ul=> ul.LongUrl.Contains(LongUrl)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a URL by its short URL code
        /// </summary>
        /// <param name="ShortUrl">The short URL code to find</param>
        /// <returns>URL object containing the short URL or null if not found</returns>
        public async Task<Url?> GetByShort(string ShortUrl)
        {
            // Find URL where the short URL contains the provided string
            return await _context.urls.Where(us=> us.ShortUrl.Contains(ShortUrl)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a short URL already exists in the database
        /// </summary>
        /// <param name="shortUrl">The short URL code to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public bool ShortExist(string shortUrl)
        {
            // Check if any URL contains the provided short URL
            return _context.urls.Any(us=> us.ShortUrl.Contains(shortUrl)); 
        }
    }
}