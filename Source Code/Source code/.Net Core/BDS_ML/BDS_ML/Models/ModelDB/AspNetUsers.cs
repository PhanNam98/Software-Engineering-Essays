using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            Admin = new HashSet<Admin>();
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            Customer = new HashSet<Customer>();
            Post = new HashSet<Post>();
            Post_Comment = new HashSet<Post_Comment>();
            Post_Favorite = new HashSet<Post_Favorite>();
            Post_Status = new HashSet<Post_Status>();
            Vip_Status = new HashSet<Vip_Status>();
        }

        [StringLength(400)]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public int IsAdmin { get; set; }
        public int IsBlock { get; set; }

        [InverseProperty("Account_")]
        public virtual ICollection<Admin> Admin { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        [InverseProperty("Account_")]
        public virtual ICollection<Customer> Customer { get; set; }
        [InverseProperty("ID_AccountNavigation")]
        public virtual ICollection<Post> Post { get; set; }
        [InverseProperty("ID_UserNavigation")]
        public virtual ICollection<Post_Comment> Post_Comment { get; set; }
        [InverseProperty("ID_UserNavigation")]
        public virtual ICollection<Post_Favorite> Post_Favorite { get; set; }
        [InverseProperty("ID_AccountNavigation")]
        public virtual ICollection<Post_Status> Post_Status { get; set; }
        [InverseProperty("ID_UserNavigation")]
        public virtual ICollection<Vip_Status> Vip_Status { get; set; }
    }
}
