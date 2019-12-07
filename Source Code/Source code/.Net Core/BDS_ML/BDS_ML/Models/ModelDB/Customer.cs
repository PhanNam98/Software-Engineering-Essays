using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Customer
    {
        public Customer()
        {
            Block = new HashSet<Block>();
            Delete_Account = new HashSet<Delete_Account>();
        }

        [Key]
        public int ID_User { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(400)]
        public string Account_ID { get; set; }
        [StringLength(100)]
        public string Avatar_URL { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("Account_ID")]
        [InverseProperty("Customer")]
        public virtual AspNetUsers Account_ { get; set; }
        [InverseProperty("ID_UserNavigation")]
        public virtual ICollection<Block> Block { get; set; }
        [InverseProperty("ID_UserNavigation")]
        public virtual ICollection<Delete_Account> Delete_Account { get; set; }
    }
}
