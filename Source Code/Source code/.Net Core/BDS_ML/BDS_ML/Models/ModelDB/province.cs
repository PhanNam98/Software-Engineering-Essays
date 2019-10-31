using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class province
    {
        public province()
        {
            Post_Location = new HashSet<Post_Location>();
            district = new HashSet<district>();
            project = new HashSet<project>();
            street = new HashSet<street>();
            ward = new HashSet<ward>();
        }

        public int id { get; set; }
        [StringLength(50)]
        public string _name { get; set; }
        [StringLength(20)]
        public string _code { get; set; }

        [InverseProperty("Tinh_TPNavigation")]
        public virtual ICollection<Post_Location> Post_Location { get; set; }
        [InverseProperty("_province_")]
        public virtual ICollection<district> district { get; set; }
        [InverseProperty("_province_")]
        public virtual ICollection<project> project { get; set; }
        [InverseProperty("_province_")]
        public virtual ICollection<street> street { get; set; }
        [InverseProperty("_province_")]
        public virtual ICollection<ward> ward { get; set; }
    }
}
