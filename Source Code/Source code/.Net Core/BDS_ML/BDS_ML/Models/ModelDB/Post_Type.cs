using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Type
    {
        public Post_Type()
        {
            Post = new HashSet<Post>();
        }

        [Key]
        public int ID_PostType { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        [InverseProperty("PostTypeNavigation")]
        public virtual ICollection<Post> Post { get; set; }
    }
}
