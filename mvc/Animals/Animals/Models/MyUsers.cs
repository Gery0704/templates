using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animals.Models
{
    public class MyUsers : IdentityUser<int>
    {
        public string FullName { get; set; }
        public Boolean tender { get; set; }
    }
}
