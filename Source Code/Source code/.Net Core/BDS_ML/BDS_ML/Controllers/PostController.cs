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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using BDS_ML.Respository;

namespace BDS_ML.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
         
        private IPostRepository _postRepository;

        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(UserManager<ApplicationUser> userManager)
        {
            this._postRepository = new PostRespository();

            _context = new BDT_MLDBContext();
            _userManager = userManager;

            listPrice = new List<priceFromToPost>();
            listPrice.Add(new priceFromToPost { key = 0, PriceFrom = 0, PriceTo = 100 });
            listPrice.Add(new priceFromToPost { key = 1, PriceFrom = 101, PriceTo = 500 });
            listPrice.Add(new priceFromToPost { key = 2, PriceFrom = 501, PriceTo = 1000 });
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

        [Authorize]
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
        [Authorize]
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


            Post_Location postlocation = new Post_Location();

            postlocation.ID_Post = post.ID_Post;
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
            postlocation.DuAn = post.Project;
            postlocation.DiaChi = diachi;


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


            try
            {

                await _context.SaveChangesAsync();
                StatusMessage = "Đăng bài thành công!";
                return RedirectToAction("Index", "Post", new { area = "ManagePosts" });

            }
            catch (Exception e)
            {
                string m = e.Message;
                StatusMessage = "Error Đăng bài không thành công";
            }


            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["Project"] = new SelectList(_context.project, "id", "_name");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["IDAccount"] = user.Id;
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            return View(post);
        }

        [AllowAnonymous]
        [HttpGet("/post/{id}")]
        public async Task<IActionResult> PostDetail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }



            //var post = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
            //              .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
            //              .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
            //              .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
            //              .Include(d => d.Post_Detail).
            //              Include(image => image.Post_Image)
            //              .Include(p => p.Post_Status)
            //              .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.ID_Post == id).SingleOrDefaultAsync();
            var post = _postRepository.GetPostByID(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _context.AspNetUsers.Where(p => p.Id == post.ID_Account).SingleOrDefaultAsync();
            PostIndex postIndex = new PostIndex();
            int count_canban = 0;
            int count_canmua = 0;
            int count_canthue = 0;
            int count_canchothue = 0;

            count_canban = postIndex.getCount(1);
            count_canmua = postIndex.getCount(3);
            count_canthue = postIndex.getCount(4);
            count_canchothue = postIndex.getCount(2);

            ViewBag.canban = count_canban;
            ViewBag.canmua = count_canmua;
            ViewBag.canthue = count_canthue;
            ViewBag.canchothue = count_canchothue;

            if (user.IsAdmin == 1)
            {
                ViewData["image"] = _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().Avatar_URL;
            }
            else
            {
                ViewData["image"] = _context.Customer.Where(p => p.Account_ID == user.Id).SingleOrDefault().Avatar_URL;
            }
            if (ViewData["image"] == null)
                ViewData["image"] = "avatar_common.png";
            var userCurrent = await _userManager.GetUserAsync(User);
            if (userCurrent != null)
            {
                ViewBag.favoritepost = _context.Post_Favorite.Include(p => p.ID_PostNavigation).ThenInclude(p => p.Post_Image).Include(p => p.ID_PostNavigation).ThenInclude(p => p.PostTypeNavigation).Include(p => p.ID_PostNavigation).ThenInclude(p => p.RealEstateTypeNavigation).Include(p => p.ID_UserNavigation)
              .Where(p => p.ID_User == user.Id).OrderByDescending(p => p.MortifiedDate).Take(5).ToList();
            }
            return View(post);
        }
       
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Search()
        {

            TempData.Remove("StatusMessage");
            ViewData["SearchKey"] = "";
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name", 1);
            ViewData["PriceFromTo"] = new SelectList(listprice, "Value", "Text");
            ViewData["District"] = new SelectList(_context.district.Where(p => p._province_id == 1).OrderBy(p => p._name), "id", "_name");
            ViewData["Ward"] = new SelectList(_context.ward.Where(p => p._province_id == 1 && p._district_id == 10).OrderBy(p => p._name), "id", "_name");
            ViewData["Street"] = new SelectList(_context.street.Where(p => p._province_id == 1 && p._district_id == 10).OrderBy(p => p._name), "id", "_name");

            return View();
        }

        public List<SelectListItem> listprice;

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Search(int district, int ward, int street, int province, string searchkey, int postType, int realestateType, int priceFromTo)
        {

            TempData.Remove("StatusMessage");
            ViewData["SearchKey"] = searchkey;
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description", postType);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description", realestateType);
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            ViewData["District"] = new SelectList(_context.district.Where(p => p._province_id == province).OrderBy(p => p._name), "id", "_name", district);
            ViewData["Ward"] = new SelectList(_context.ward.Where(p => p._province_id == province && p._district_id == district).OrderBy(p => p._name), "id", "_name", ward);
            ViewData["Street"] = new SelectList(_context.street.Where(p => p._province_id == province && p._district_id == district).OrderBy(p => p._name), "id", "_name", street);
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
              .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == postType && p.RealEstateType == realestateType
              && p.Tittle.Contains(searchkey)
              //&& p.Post_Location.LastOrDefault().Tinh_TP.Value.ToString() == province.ToString()
              // && p.Post_Location.LastOrDefault().Quan_Huyen.Value == district
              // && p.Post_Location.LastOrDefault().Phuong_Xa.Value == ward && p.Post_Location.SingleOrDefault().Duong_Pho.Value == street
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo
              ).ToListAsync();
                                List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Phuong_Xa == ward
                               && p.Post_Location.SingleOrDefault().Quan_Huyen == district && p.Post_Location.SingleOrDefault().Duong_Pho == street).ToList();

                                ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                                return View(a);
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
             //&& p.Tittle.Contains(searchkey)
             //&& p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
             //&& p.Post_Location.SingleOrDefault().Phuong_Xa == ward 
             && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();

                                List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Phuong_Xa == ward
                              && p.Post_Location.SingleOrDefault().Quan_Huyen == district).ToList();
                                ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                                return View(a);
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
             && p.Tittle.Contains(searchkey)
              //&& p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                            List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province
                             && p.Post_Location.SingleOrDefault().Quan_Huyen == district).ToList();
                            ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                            return View(a);
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
             && p.Tittle.Contains(searchkey)
             //&& p.Post_Location.SingleOrDefault().Tinh_TP == province 
             && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                        List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province).ToList();
                        ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                        return View(a);
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

              //&& p.Post_Location.Single().Tinh_TPNavigation.id == province
              //&& p.Post_Location.SingleOrDefault().Quan_Huyen.Value == district
              //&& p.Post_Location.SingleOrDefault().Phuong_Xa.Value == ward && p.Post_Location.SingleOrDefault().Duong_Pho.Value == street
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo
              ).ToListAsync();
                                List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Phuong_Xa == ward
                                && p.Post_Location.SingleOrDefault().Quan_Huyen == district && p.Post_Location.SingleOrDefault().Duong_Pho == street).ToList();
                                ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                                return View(a);
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
             // && p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
             //&& p.Post_Location.SingleOrDefault().Phuong_Xa == ward 
             && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                                List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Phuong_Xa == ward
                               && p.Post_Location.SingleOrDefault().Quan_Huyen == district).ToList();
                                ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                                return View(a);
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
              //&& p.Post_Location.SingleOrDefault().Tinh_TP == province && p.Post_Location.SingleOrDefault().Quan_Huyen == district
              && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                            List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province
                               && p.Post_Location.SingleOrDefault().Quan_Huyen == district).ToList();
                            ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                            return View(a);
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
            //&& p.Post_Location.SingleOrDefault().Tinh_TP == province
            && p.Price >= listPrice[priceFromTo].PriceFrom && p.Price <= listPrice[priceFromTo].PriceTo).ToListAsync();
                        List<Post> a = posts.Where(p => p.Post_Location.SingleOrDefault().Tinh_TP == province).ToList();
                        ViewData["StatusResult"] = "Tìm thấy " + a.Count() + " kết quả";
                        return View(a);
                    }
                }

            }
            catch (Exception e)
            {
                string a = e.Message;
                ViewData["StatusResult"] = "Error Tìm không thành công";

            }


            ViewData["StatusResult"] = "Error Không tìm thấy kết quả phù hợp";
            return View();
        }

        //Thêm bài đăng vào danh sách yêu thích
        [HttpPost]
        public async Task<JsonResult> AddFavorite(int idpost)
        {
            var user = await _userManager.GetUserAsync(User);

            var post_Favoritecount = _context.Post_Favorite.Where(p => p.ID_Post == idpost && p.ID_User == user.Id).Count();
            if (post_Favoritecount > 0)
            {
                return Json(new { Result = "ERROR", Message = "Đã có trong danh sách yêu thích" });
            }
            try
            {
                Post_Favorite post_Favorite = new Post_Favorite();
                post_Favorite.ID_Post = idpost;
                post_Favorite.ID_User = user.Id;
                post_Favorite.MortifiedDate = DateTime.Now;
                _context.Post_Favorite.Add(post_Favorite);
                _context.SaveChanges();
                return Json(new { Result = "OK", Message = "Đã thêm vào danh sách yêu thích" });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }

        [HttpPost]
        public async Task<JsonResult> RemoveFavorite(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var post_Favoritecount = _context.Post_Favorite.Where(p => p.ID_Post == id && p.ID_User == user.Id).Count();
            if (post_Favoritecount == 0)
            {
                return Json(new { Result = "ERROR", Message = "Bài đăng không có trong danh sách yêu thích" });
            }
            try
            {
                Post_Favorite post_Favorite = _context.Post_Favorite.Where(p => p.ID_Post == id && p.ID_User == user.Id).SingleOrDefault();
                _context.Post_Favorite.Remove(post_Favorite);
                _context.SaveChanges();
                return Json(new { Result = "OK", Message = "Đã xóa bài ra khỏi danh sách yêu thích", Id = id });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }



        //báo cáo bài đăng 
        [HttpPost]
        public async Task<JsonResult> AddReportPost(int idpost,string reason)
        {
            var user = await _userManager.GetUserAsync(User);
           
            var post_Reporrtcount = _context.Report_Post.Where(p => p.ID_Post == idpost && p.ID_Account_Report==user.Id && p.MortifiedDate.Date== DateTime.Now.Date).Count();
            if (post_Reporrtcount > 0)
            {
                return Json(new { Result = "ERROR", Message = "Bạn đã báo cáo bài đăng này trong hôm nay rồi!\n" });
            }
            try
            {
                Report_Post report = new Report_Post();
                report.ID_Post = idpost;
                report.ID_Account_Report = user.Id;
                report.MortifiedDate = DateTime.Now;
                report.Reason = reason;
                report.IsRead = false;
                _context.Report_Post.Add(report);
                _context.SaveChanges();
                return Json(new { Result = "OK", Message = "Đã báo cáo bài đăng" });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }

        //get post by post type large
        [AllowAnonymous]
        [HttpGet]
        [Route("mua-thue-ban-chothue")]
        public IActionResult SearchByPostType(int id = 0, int pageindex = 1)
        {
            List<Post> lstFull = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).Include(image => image.Post_Image).Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status)
             .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1).ToList();

            List<Post> lst = null;

            if (id == 0)
            {
                ViewBag.Title = "Cần Mua/Cần Thuê";
                lst = lstFull.Where(p => p.PostType == 3 || p.PostType == 4).OrderByDescending(p=>p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst,6, pageindex);
                model.Action = "SearchByPostType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }
            else
            {
                ViewBag.Title = "Cần Bán/Cho Thuê";
                lst = lstFull.Where(p => p.PostType == 1 || p.PostType == 2).OrderByDescending(p => p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst,6, pageindex);
                model.Action = "SearchByPostType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }

        }

        //get post by post type
        [AllowAnonymous]
        [Route("type/{id}")]
        public IActionResult SearchByType(int id = 0, int pageindex = 1)
        {
            List<Post> lstFull = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
             .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
             .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
             .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
             .Include(d => d.Post_Detail).Include(image => image.Post_Image).Include(p => p.Post_Status)
             .ThenInclude(pt => pt.StatusNavigation.Post_Status)
             .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1).ToList();

            List<Post> lst = null;

            if (id == 0)
            {
                ViewBag.Title = "Cần Bán";
                lst = lstFull.Where(p => p.PostType == 1).OrderByDescending(p=>p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst, 6, pageindex);
                model.Action = "SearchByType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }
            else
            if (id == 1)
            {
                ViewBag.Title = "Cần Cho Thuê";
                lst = lstFull.Where(p => p.PostType == 2).OrderByDescending(p => p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst, 6, pageindex);
                model.Action = "SearchByType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }
            else
            if (id == 2)
            {
                ViewBag.Title = "Cần Mua";
                lst = lstFull.Where(p => p.PostType == 3).OrderByDescending(p => p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst, 6, pageindex);
                model.Action = "SearchByType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }
            else
            {
                ViewBag.Title = "Cần Cho Thuê";
                lst = lstFull.Where(p => p.PostType == 4).OrderByDescending(p => p.PostTime).ToList();
                ViewBag.Count = lst.Count();
                var model = PagingList.Create(lst, 6, pageindex);
                model.Action = "SearchByType";
                model.RouteValue = new RouteValueDictionary {
                { "id", id}};
                return View("Index", model);
            }

          
        }

        //get post by post type
        //[AllowAnonymous]
        //[Route("hot-location/{id}")]
        //public IActionResult SearchByLocation(string lc)
        //{
        //    List<Post> lstFull = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
        //     .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
        //     .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
        //     .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
        //     .Include(d => d.Post_Detail).Include(image => image.Post_Image).Include(p => p.Post_Status)
        //     .ThenInclude(pt => pt.StatusNavigation.Post_Status)
        //     .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1).ToList();

        //    List<Post> lst = null;
        //    ViewBag.Title = lc;

        //    lst = lstFull.Where(p => p.Post_Location.SingleOrDefault().Tinh_TPNavigation._name == lc).ToList();

        //    return View("Index", lst);
        //}
    }
}