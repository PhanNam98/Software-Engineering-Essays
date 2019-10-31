using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class RealEstate_Type
    {
        public RealEstate_Type()
        {
            Post = new HashSet<Post>();
        }

        [Key]
        public int ID_RealEstateType { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        [InverseProperty("RealEstateTypeNavigation")]
        public virtual ICollection<Post> Post { get; set; }
    }
}
