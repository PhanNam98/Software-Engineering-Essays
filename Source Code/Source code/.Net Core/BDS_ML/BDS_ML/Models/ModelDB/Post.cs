using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post
    {
        public Post()
        {
            Post_Comment = new HashSet<Post_Comment>();
            Post_Detail = new HashSet<Post_Detail>();
            Post_Favorite = new HashSet<Post_Favorite>();
            Post_Image = new HashSet<Post_Image>();
            Post_Location = new HashSet<Post_Location>();
            Post_Status = new HashSet<Post_Status>();
            Report_Post = new HashSet<Report_Post>();
        }

        [Key]
        public int ID_Post { get; set; }
        [Required]
        [StringLength(400)]
        public string ID_Account { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PostTime { get; set; }
        public int PostType { get; set; }
        [StringLength(250)]
        public string Tittle { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Size { get; set; }
        public int? Project { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int RealEstateType { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }

        [ForeignKey("ID_Account")]
        [InverseProperty("Post")]
        public virtual AspNetUsers ID_AccountNavigation { get; set; }
        [ForeignKey("PostType")]
        [InverseProperty("Post")]
        public virtual Post_Type PostTypeNavigation { get; set; }
        [ForeignKey("Project")]
        [InverseProperty("Post")]
        public virtual project ProjectNavigation { get; set; }
        [ForeignKey("RealEstateType")]
        [InverseProperty("Post")]
        public virtual RealEstate_Type RealEstateTypeNavigation { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Comment> Post_Comment { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Detail> Post_Detail { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Favorite> Post_Favorite { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Image> Post_Image { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Post_Status> Post_Status { get; set; }
        [InverseProperty("ID_PostNavigation")]
        public virtual ICollection<Report_Post> Report_Post { get; set; }
    }
}
