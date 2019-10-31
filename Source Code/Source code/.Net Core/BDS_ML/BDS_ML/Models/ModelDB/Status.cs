using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Status
    {
        public Status()
        {
            Post_Status = new HashSet<Post_Status>();
        }

        [Key]
        public int ID_Status { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        [InverseProperty("StatusNavigation")]
        public virtual ICollection<Post_Status> Post_Status { get; set; }
    }
}
