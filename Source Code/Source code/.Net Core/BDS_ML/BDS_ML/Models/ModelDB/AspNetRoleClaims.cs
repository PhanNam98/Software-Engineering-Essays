using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class AspNetRoleClaims
    {
        public int Id { get; set; }
        [Required]
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("AspNetRoleClaims")]
        public virtual AspNetRoles Role { get; set; }
    }
}
