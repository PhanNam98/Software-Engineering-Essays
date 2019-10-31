using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Favorite
    {
        [StringLength(400)]
        public string ID_User { get; set; }
        public int ID_Post { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MortifiedDate { get; set; }

        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Favorite")]
        public virtual Post ID_PostNavigation { get; set; }
        [ForeignKey("ID_User")]
        [InverseProperty("Post_Favorite")]
        public virtual AspNetUsers ID_UserNavigation { get; set; }
    }
}
