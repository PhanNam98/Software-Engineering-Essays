using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) {
        }
        public ApplicationRole(string roleName,string description,DateTime creationDate) : base(roleName) {
            this.CreationDate = creationDate;
            this.Description = description;
        }
        public string Description;
        public DateTime CreationDate;
    }
}
