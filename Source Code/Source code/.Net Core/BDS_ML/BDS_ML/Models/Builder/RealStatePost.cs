using BDS_ML.Data;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.Builder
{
    public class RealStatePost
    {
        Post_Status poststatus;
        Post_Detail postdetail;
        Post_Location postlocation;

        public async void Add_Post_Status(string ID_User,int ID_Post,int Role)
        {
            this.poststatus = new Post_Status();
            poststatus.ID_Account = ID_User;
            poststatus.ID_Post = ID_Post;

            poststatus.ModifiedDate = DateTime.Now;
            if (Role == 1)
            {
                poststatus.Status = 1;
            }
            else
            {
                poststatus.Status = 5;
            }
            DataProvider.Ins.db.Post_Status.Add(poststatus);

            await DataProvider.Ins.db.SaveChangesAsync();
            
        }
        public void Add_Post_Detail(int ID_Post, bool alley, int bedroom, int yard, int floor, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket,
            string descriptiondetail, int bathroom)
        {
            postdetail = new Post_Detail()
            {
                ID_Post = ID_Post,
                Alley = alley,
                Bathroom = bathroom,
                Bedroom = bedroom,
                Description = descriptiondetail,
                Floor = floor,
                Yard = yard,
                NearHospital = nearHospital,
                NearAirport = nearAirport,
                NearSchool = nearSchool,
                NearMarket = nearMarket,
            };
            DataProvider.Ins.db.Post_Detail.Add(postdetail);
            DataProvider.Ins.db.SaveChanges();
        }
        public void Add_Post_Location(int ID_Post, int district, int ward, int street, string diachi, int province, int? project)
        {
            postlocation = new Post_Location();

            postlocation.ID_Post = ID_Post;
            postlocation.Tinh_TP = province;
            postlocation.Quan_Huyen = district;
            if (ward == 0)
            {
                postlocation.Phuong_Xa = null;
            }
            else
                postlocation.Phuong_Xa = ward;

            if (street == 0)
            {
                postlocation.Duong_Pho = null;
            }
            else
                postlocation.Duong_Pho = street;
            postlocation.DuAn = project;
            postlocation.DiaChi = diachi;


            DataProvider.Ins.db.Post_Location.Add(postlocation);
            DataProvider.Ins.db.SaveChanges();
        }
        public async void Add_Post_Image(int ID_Post,string ID_User,List<IFormFile> images)
        {
            if (images.Count > 0 && images[0].Length > 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var file = images[i];

                    if (file != null && images[i].Length > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string extensionFileName = Path.GetExtension(fileName);
                        if (fileName.Length - extensionFileName.Length > 40)
                        {
                            fileName = fileName.Substring(0, 40) + "-" + ID_User + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                        }

                        else

                            fileName = fileName.Substring(0, fileName.Length - extensionFileName.Length) + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\posts", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Post_Image pstImg = new Post_Image();
                        pstImg.url = fileName;
                        pstImg.AddedDate = DateTime.Now;
                        pstImg.ID_Post = ID_Post;
                        DataProvider.Ins.db.Post_Image.Add(pstImg);
                        DataProvider.Ins.db.SaveChanges();
                    }
                }

            }
           
        }

    }
}
