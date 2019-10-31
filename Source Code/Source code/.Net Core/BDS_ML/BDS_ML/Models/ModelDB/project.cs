using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class project
    {
        public project()
        {
            Post = new HashSet<Post>();
            Post_Location = new HashSet<Post_Location>();
        }

        public int id { get; set; }
        [StringLength(200)]
        public string _name { get; set; }
        public int? _province_id { get; set; }
        public int? _district_id { get; set; }
        public double? _lat { get; set; }
        public double? _lng { get; set; }

        [ForeignKey("_district_id")]
        [InverseProperty("project")]
        public virtual district _district_ { get; set; }
        [ForeignKey("_province_id")]
        [InverseProperty("project")]
        public virtual province _province_ { get; set; }
        [InverseProperty("ProjectNavigation")]
        public virtual ICollection<Post> Post { get; set; }
        [InverseProperty("DuAnNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
    }
}
