using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Detail
    {
        public int ID_Detail { get; set; }
        public int ID_Post { get; set; }
        public int? Floor { get; set; }
        public int? Bedroom { get; set; }
        public int? Bathroom { get; set; }
        public int? Yard { get; set; }
        public bool? NearHospital { get; set; }
        public bool? NearMarket { get; set; }
        public bool? NearAirport { get; set; }
        public bool? NearSchool { get; set; }
        public bool? Alley { get; set; }
        [StringLength(300)]
        public string Description { get; set; }

        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Detail")]
        public virtual Post ID_PostNavigation { get; set; }
    }
}
