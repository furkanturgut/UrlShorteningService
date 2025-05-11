using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Interfaces
{
    public interface IUrlService
    {
        Task<Url?> Create (CreateUrlDto urlDto);
        Task<IEnumerable<ReturnUrlDto>?> GetAll ();
        Task<Url?> GetByLong(string longUrl);

        Task<Url?> GetByShort(string shortUrl);
        Task<Url?> Delete (int Id);
    }
}