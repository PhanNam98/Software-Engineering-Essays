using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Vip_Status
    {
        public int ID_Vip { get; set; }
        [StringLength(400)]
        public string ID_User { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ActiveDay { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }

        [ForeignKey("ID_User")]
        [InverseProperty("Vip_Status")]
        public virtual AspNetUsers ID_UserNavigation { get; set; }
    }
}
