using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class ward
    {
        public ward()
        {
            Post_Location = new HashSet<Post_Location>();
        }

        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string _name { get; set; }
        [StringLength(20)]
        public string _prefix { get; set; }
        public int? _province_id { get; set; }
        public int? _district_id { get; set; }

        [ForeignKey("_district_id")]
        [InverseProperty("ward")]
        public virtual district _district_ { get; set; }
        [ForeignKey("_province_id")]
        [InverseProperty("ward")]
        public virtual province _province_ { get; set; }
        [InverseProperty("Phuong_XaNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
    }
}
