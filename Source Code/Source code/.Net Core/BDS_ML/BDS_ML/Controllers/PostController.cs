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
using Microsoft.EntityFrameworkCore;
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
           
             listPrice = new List<priceFromToPost>();
            listPrice.Add(new priceFromToPost { key = 0, PriceFrom = 0, PriceTo = 100 });
            listPrice.Add(new priceFromToPost { key = 1, PriceFrom = 101, PriceTo = 500 });
            listPrice.Add(new priceFromToPost { key = 2, PriceFrom = 5001, PriceTo = 1000 });
            listPrice.Add(new priceFromToPost { key = 3, PriceFrom = 1001, PriceTo = 3000 });
            listPrice.Add(new priceFromToPost { key = 4, PriceFrom = 3001, PriceTo = 10000 });
            listPrice.Add(new priceFromToPost { key = 5, PriceFrom = 10001, PriceTo = 1000000 });
            listprice = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Dưới 100 triệu", Value = (0).ToString()},
                    new SelectListItem { Text = "Từ 100 triệu đến 500 triệu", Value = (1).ToString()},
                    new SelectListItem { Text = "Từ 500 triệu đến 1 tỷ", Value = (2).ToString()},
                    new SelectListItem { Text = "Từ 1 tỷ đến 3 tỷ", Value = (3).ToString()},
                    new SelectListItem { Text = "Từ 3 tỷ đến 10 tỷ", Value = (4).ToString()},
                     new SelectListItem { Text = "Trên 10 tỷ", Value = (5).ToString()}
                };
        }
        [TempData]
        public string StatusMessage { get; set; }
       
        public class priceFromToPost
        {
            public int key { get; set; }
            public decimal PriceFrom { get; set; }
            public decimal PriceTo { get; set; }
        }
        public List<priceFromToPost> listPrice;

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
            , int district, int ward, int street, string diachi, bool alley, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket, List<IFormFile> images, string descriptiondetail, int bathroom,
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
                Description = descriptiondetail,
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
                        if (fileName.Length - extensionFileName.Length > 40)
                        {
                            fileName = fileName.Substring(0, 40) + "-" + user.Id + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

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
        //[AllowAnonymous]
        //public JsonResult Get_district(int province_id)
        //{
        //    var list = _context.district.Where(p => p._province_id == province_id);
        //    return Json(list.Select(x => new
        //    {
        //        ID = x.id,
        //        Name = x._prefix + " " + x._name
        //    }).ToList());
        //}
        //[AllowAnonymous]
        //public JsonResult Get_ward(int province_id, int district_id)
        //{
        //    var list = _context.ward.Where(p => p._province_id == province_id && p._district_id == district_id);
        //    return Json(list.Select(x => new
        //    {
        //        ID = x.id,
        //        Name = x._prefix + " " + x._name
        //    }).ToList());
        //}
        //[AllowAnonymous]
        //public JsonResult Get_street(int province_id, int district_id)
        //{
        //    var list = _context.street.Where(p => p._province_id == province_id && p._district_id == district_id);
        //    return Json(list.Select(x => new
        //    {
        //        ID = x.id,
        //        Name = x._prefix + " " + x._name
        //    }).ToList());
        //}
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Search()
        {

            TempData.Remove("StatusMessage");
            ViewData["SearchKey"] = "";
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            ViewData["PriceFromTo"] = new SelectList(listprice, "Value", "Text");
            return View();
        }
        public List<SelectListItem> listprice;
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Search(int district, int ward, int street, int province, string searchkey, int postType, int realestateType, int priceFromTo)
        {

            TempData.Remove("StatusMessage");
            ViewData["SearchKey"] = "";
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description", postType);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description", realestateType);
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            ViewData["PriceFromTo"] = new SelectList(listprice, "Value", "Text", priceFromTo);
            try
            {
                if (searchkey != "" && searchkey != null)
                {
                    if (district != 0)
                    {
                        if (ward != 0)
                        {
                            if (street != 0)
                            {
                                var posts = await _context.Post.Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
              .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
              .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .
              Include(image => image.Post_Image)
              .Include(p => p.Post_Status)
              .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p =>p.Post_Status.OrderBy(c=>c.ModifiedDate).LastOrDefault().Status==1 && p.PostType == postType && p.RealEstateType == realestateType
              && p.Tittle.Contains(searchkey)
              && p.Post_Location.SingleOrDefault().Tinh_TP.Value == province
              && p.Post_Location.SingleOrDefault().Quan_Huyen.Value == district
              && p.Post_Location.SingleOrDefault().Phuong_Xa.Value == ward && p.Post_Location.SingleOrDefault().Duong_Pho.Value == street
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo
              ).ToListAsync();
                                ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                                return View(posts);
                            }
                            else
                            {
                                var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
             && p.Tittle.Contains(searchkey) && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
             && p.Post_Location.SingleOrDefault().Phuong_Xa == ward && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                                ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                                return View(posts);
                            }

                        }
                        else
                        {
                            var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
             && p.Tittle.Contains(searchkey) && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                            ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                            return View(posts);
                        }
                    }
                    else
                    {
                        var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
             && p.Tittle.Contains(searchkey) && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                        ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                        return View(posts);
                    }
                }
                else
                {
                    if (district != 0)
                    {
                        if (ward != 0)
                        {
                            if (street != 0)
                            {
                                var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
              .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
              .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
              .Include(d => d.Post_Detail).
              Include(image => image.Post_Image)
              .Include(p => p.Post_Status)
              .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType

              && p.Post_Location.SingleOrDefault().Tinh_TP.Value == province
              && p.Post_Location.SingleOrDefault().Quan_Huyen.Value == district
              && p.Post_Location.SingleOrDefault().Phuong_Xa.Value == ward && p.Post_Location.SingleOrDefault().Duong_Pho.Value == street
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo
              ).ToListAsync();
                                ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                                return View(posts);
                            }
                            else
                            {
                                var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
              && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
             && p.Post_Location.SingleOrDefault().Phuong_Xa == ward && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                                ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                                return View(posts);
                            }

                        }
                        else
                        {
                            var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
           && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                            ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                            return View(posts);
                        }
                    }
                    else
                    {
                        var posts = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).
             Include(image => image.Post_Image)
             .Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
            && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                        ViewData["StatusResult"] = "Tìm thấy " + posts.Count() + " kết quả";
                        return View(posts);
                    }
                }

            }
            catch
            {
                ViewData["StatusResult"] = "Error Tìm không thành công";
               
            }


            ViewData["StatusResult"] = "Error Không tìm thấy kết quả phù hợp";
            return View();
        }
    }
}