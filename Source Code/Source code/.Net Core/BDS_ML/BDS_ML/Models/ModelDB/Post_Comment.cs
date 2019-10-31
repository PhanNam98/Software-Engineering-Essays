using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Comment
    {
        public int ID_Post { get; set; }
        public int Comment_ID { get; set; }
        [StringLength(400)]
        public string ID_User { get; set; }
        [StringLength(400)]
        public string Comment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time { get; set; }

        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Comment")]
        public virtual Post ID_PostNavigation { get; set; }
        [ForeignKey("ID_User")]
        [InverseProperty("Post_Comment")]
        public virtual AspNetUsers ID_UserNavigation { get; set; }
    }
}
