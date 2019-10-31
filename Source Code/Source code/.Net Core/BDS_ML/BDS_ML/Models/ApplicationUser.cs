using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser() : base() { }

        public int IsBlock { get; set; }
        public int IsAdmin { get; set; }

    }
}
