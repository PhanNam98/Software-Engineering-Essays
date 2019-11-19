using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDS_ML.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(UserManager<ApplicationUser> userManager)
        {
            _context = new BDT_MLDBContext();
            _userManager = userManager;
            StatusMessage = "Đang xử lí";
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("~/Identity/Account/Login?returnUrl=~/post/create");
            }
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["Project"] = new SelectList(_context.project, "id", "_name");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["IDAccount"] = user.Id;
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            return View();
        }

        // POST: Admin/ManagePostsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post, string id
            , int district, int ward, int street, string diachi, bool alley, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket, List<IFormFile> images, string description, int bathroom,
            int bedroom, int yard, int floor, int province)
        {

            var user = await _userManager.GetUserAsync(User);
            post.PostTime = DateTime.Now;
            post.ID_Account = user.Id;
            Post a = post;

            _context.Post.Add(post);
            await _context.SaveChangesAsync();
            Post_Status poststatus = new Post_Status();
            poststatus.ID_Account = user.Id;
            poststatus.ID_Post = post.ID_Post;

            poststatus.ModifiedDate = DateTime.Now;
            if (user.IsAdmin == 1)
            {
                poststatus.Status = 1;
            }
            else
            {
                poststatus.Status = 5;
            }
            _context.Post_Status.Add(poststatus);

            await _context.SaveChangesAsync();
            Post_Detail postdetail = new Post_Detail()
            {
                ID_Post = post.ID_Post,
                Alley = alley,
                Bathroom = bathroom,
                Bedroom = bedroom,
                Description = description,
                Floor = floor,
                Yard = yard,
                NearHospital = nearHospital,
                NearAirport = nearAirport,
                NearSchool = nearSchool,
                NearMarket = nearMarket,
            };
            //}
            Post_Location postlocation = new Post_Location()
            {
                ID_Post = post.ID_Post,
                Tinh_TP = province,
                Quan_Huyen = district,
                Phuong_Xa = ward,
                Duong_Pho = street,
                DuAn = post.Project,
                DiaChi = diachi

            };

            _context.Post_Location.Add(postlocation);
           
            _context.Post_Detail.Add(postdetail);
          
            if (images.Count > 0 && images[0].Length > 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var file = images[i];

                    if (file != null && images[i].Length > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);

                        string extensionFileName = Path.GetExtension(fileName);

                        fileName = fileName.Substring(0, fileName.Length - extensionFileName.Length) + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\posts", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Post_Image pstImg = new Post_Image();
                        pstImg.url = fileName;
                        pstImg.AddedDate = DateTime.Now;
                        pstImg.ID_Post = post.ID_Post;
                       _context.Post_Image.Add(pstImg);
                       
                    }
                }

            }
            
            //if (post.RealEstateType == 1)
            //{
           
            //_context.SaveChanges();
            //post.Status = poststatus.ID_PostStatus;
            try
            {
                //_context.Post.Attach(post);
                //_context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync();
                if (user.IsAdmin == 1)
                {
                    StatusMessage = "Đăng bài thành công!";
                    return RedirectToAction("Index", "ManagePostsAdmin", new { area = "Admin" });
                }
                else

                {
                    StatusMessage = "Đăng bài thành công! Xin chờ admin duyệt";
                    return RedirectToAction("Index", "ManagePostsAdmin", new { area = "User" });
                }
            }
            catch 
            {
                StatusMessage = "Error Đăng bài không thành công";
            }


            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["Project"] = new SelectList(_context.project, "id", "_name");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["IDAccount"] = user.Id;
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            return View(post);
        }
        public JsonResult Get_district(int province_id)
        {
            var list = _context.district.Where(p => p._province_id == province_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }
        public JsonResult Get_ward(int province_id, int district_id)
        {
            var list = _context.ward.Where(p => p._province_id == province_id && p._district_id == district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }
        public JsonResult Get_street(int province_id, int district_id)
        {
            var list = _context.street.Where(p => p._province_id == province_id && p._district_id == district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }


    }
}