using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos
{
    public class CreateUrlDto
    {
        public string? customAlias { get; set; } = null;
        [Required]
        public string originalUrl { get; set; }
       
    }
}