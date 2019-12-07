using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Admin
    {
        public Admin()
        {
            Block = new HashSet<Block>();
            Delete_Account = new HashSet<Delete_Account>();
        }

        [Key]
        public int ID_Admin { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        [StringLength(20)]
        public string IDNumber { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Avatar_URL { get; set; }
        [Required]
        [StringLength(400)]
        public string Account_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("Account_ID")]
        [InverseProperty("Admin")]
        public virtual AspNetUsers Account_ { get; set; }
        [InverseProperty("ID_AdminNavigation")]
        public virtual ICollection<Block> Block { get; set; }
        [InverseProperty("ID_AdminNavigation")]
        public virtual ICollection<Delete_Account> Delete_Account { get; set; }
    }
}
