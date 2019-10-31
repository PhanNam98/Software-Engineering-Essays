using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Status
    {
        [Key]
        public int ID_PostStatus { get; set; }
        public int? ID_Post { get; set; }
        public int Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(500)]
        public string Reason { get; set; }
        [StringLength(400)]
        public string ID_Account { get; set; }

        [ForeignKey("ID_Account")]
        [InverseProperty("Post_Status")]
        public virtual AspNetUsers ID_AccountNavigation { get; set; }
        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Status")]
        public virtual Post ID_PostNavigation { get; set; }
        [ForeignKey("Status")]
        [InverseProperty("Post_Status")]
        public virtual Status StatusNavigation { get; set; }
    }
}
