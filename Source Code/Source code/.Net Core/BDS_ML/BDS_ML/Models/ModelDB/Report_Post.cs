using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Report_Post
    {
        public int ID_Report { get; set; }
        [StringLength(400)]
        public string ID_Account_Report { get; set; }
        public int ID_Post { get; set; }
        public bool? IsRead { get; set; }
        [Required]
        [StringLength(256)]
        public string Reason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime MortifiedDate { get; set; }

        [ForeignKey("ID_Account_Report")]
        [InverseProperty("Report_Post")]
        public virtual AspNetUsers ID_Account_ReportNavigation { get; set; }
        [ForeignKey("ID_Post")]
        [InverseProperty("Report_Post")]
        public virtual Post ID_PostNavigation { get; set; }
    }
}
