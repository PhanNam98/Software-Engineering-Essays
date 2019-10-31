using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class district
    {
        public district()
        {
            Post_Location = new HashSet<Post_Location>();
            project = new HashSet<project>();
            street = new HashSet<street>();
            ward = new HashSet<ward>();
        }

        public int id { get; set; }
        [StringLength(100)]
        public string _name { get; set; }
        [StringLength(20)]
        public string _prefix { get; set; }
        public int? _province_id { get; set; }

        [ForeignKey("_province_id")]
        [InverseProperty("district")]
        public virtual province _province_ { get; set; }
        [InverseProperty("Quan_HuyenNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
        [InverseProperty("_district_")]
        public virtual ICollection<project> project { get; set; }
        [InverseProperty("_district_")]
        public virtual ICollection<street> street { get; set; }
        [InverseProperty("_district_")]
        public virtual ICollection<ward> ward { get; set; }
    }
}
