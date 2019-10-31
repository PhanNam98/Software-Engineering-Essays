using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDS_ML.Models.ModelDB
{
    public partial class Post_Location
    {
        public int ID_Location { get; set; }
        public int? ID_Post { get; set; }
        public int? Tinh_TP { get; set; }
        public int? Quan_Huyen { get; set; }
        public int? Phuong_Xa { get; set; }
        public int? Duong_Pho { get; set; }
        public int? DuAn { get; set; }
        [StringLength(50)]
        public string DiaChi { get; set; }

        [ForeignKey("DuAn")]
        [InverseProperty("Post_Location")]
        public virtual project DuAnNavigation { get; set; }
        [ForeignKey("Duong_Pho")]
        [InverseProperty("Post_Location")]
        public virtual street Duong_PhoNavigation { get; set; }
        [ForeignKey("ID_Post")]
        [InverseProperty("Post_Location")]
        public virtual Post ID_PostNavigation { get; set; }
        [ForeignKey("Phuong_Xa")]
        [InverseProperty("Post_Location")]
        public virtual ward Phuong_XaNavigation { get; set; }
        [ForeignKey("Quan_Huyen")]
        [InverseProperty("Post_Location")]
        public virtual district Quan_HuyenNavigation { get; set; }
        [ForeignKey("Tinh_TP")]
        [InverseProperty("Post_Location")]
        public virtual province Tinh_TPNavigation { get; set; }
    }
}
