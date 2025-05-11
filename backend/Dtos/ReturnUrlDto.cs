using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos
{
    public class ReturnUrlDto
    {
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string shortUrl { get; set; }
    }
}