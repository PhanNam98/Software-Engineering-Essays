using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Image
    {
        [Key]
        public int ID_Image { get; set; }
        public int ID_Post { get; set; }
        [StringLength(255)]
        public string url { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AddedDate { get; set; }

        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Image")]
        public virtual Post ID_PostNavigation { get; set; }
    }
}
