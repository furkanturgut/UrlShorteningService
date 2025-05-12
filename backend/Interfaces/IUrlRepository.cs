using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IUrlRepository
    {
        Task<Url?> GetById (int Id);
         Task<Url?> Create (Url url);
        Task<List<Url>?> GetAll (); 
        bool ShortExist(string shortUrl);
        Task<Url?> GetByLong (string LongUrl);
        Task<Url?> GetByShort (string ShortUrl); 
        Task<Url?> Delete (Url url); 
    }
}