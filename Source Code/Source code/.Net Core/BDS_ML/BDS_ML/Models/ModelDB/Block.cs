using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Block
    {
        public int ID_Block { get; set; }
        public int ID_User { get; set; }
        public int ID_Admin { get; set; }
        [Required]
        [StringLength(450)]
        public string Reason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BlockDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UnLockDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("ID_Admin")]
        [InverseProperty("Block")]
        public virtual Admin ID_AdminNavigation { get; set; }
        [ForeignKey("ID_User")]
        [InverseProperty("Block")]
        public virtual Customer ID_UserNavigation { get; set; }
    }
}
