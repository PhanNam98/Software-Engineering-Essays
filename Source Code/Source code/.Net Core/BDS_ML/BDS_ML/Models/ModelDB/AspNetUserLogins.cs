using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class AspNetUserLogins
    {
        [StringLength(128)]
        public string LoginProvider { get; set; }
        [StringLength(128)]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        [Required]
        [StringLength(400)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AspNetUserLogins")]
        public virtual AspNetUsers User { get; set; }
    }
}
