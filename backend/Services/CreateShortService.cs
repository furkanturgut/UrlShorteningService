using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public static class CreateShortService
    {
        public static string CreateShort()
        {
            return Guid.NewGuid().ToString("N").Substring(0,5);
        }
    }
}