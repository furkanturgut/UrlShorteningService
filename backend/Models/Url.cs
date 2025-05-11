using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(LongUrl))]
    [Index(nameof(ShortUrl))]
    public class Url
    {
        public int Id {get;set;}
        public required string LongUrl { get; set; } 
        public string? ShortUrl { get; set; } = string.Empty ; 
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    }
}