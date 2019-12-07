using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Delete_Account
    {
        public int ID_Delete { get; set; }
        public int ID_User { get; set; }
        public int? ID_Admin { get; set; }
        [StringLength(450)]
        public string Reason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeleteDate { get; set; }

        [ForeignKey("ID_Admin")]
        [InverseProperty("Delete_Account")]
        public virtual Admin ID_AdminNavigation { get; set; }
        [ForeignKey("ID_User")]
        [InverseProperty("Delete_Account")]
        public virtual Customer ID_UserNavigation { get; set; }
    }
}
