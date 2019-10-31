using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class street
    {
        public street()
        {
            Post_Location = new HashSet<Post_Location>();
        }

        public int id { get; set; }
        [StringLength(100)]
        public string _name { get; set; }
        [StringLength(20)]
        public string _prefix { get; set; }
        public int? _province_id { get; set; }
        public int? _district_id { get; set; }

        [ForeignKey("_district_id")]
        [InverseProperty("street")]
        public virtual district _district_ { get; set; }
        [ForeignKey("_province_id")]
        [InverseProperty("street")]
        public virtual province _province_ { get; set; }
        [InverseProperty("Duong_PhoNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
    }
}
