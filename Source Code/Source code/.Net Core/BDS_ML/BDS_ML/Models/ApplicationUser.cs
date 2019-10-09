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


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int IsBlock { get; set; }
        public int IsAdmin { get; set; }

    }
}
