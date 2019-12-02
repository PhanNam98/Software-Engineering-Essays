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

namespace BDS_ML.Areas.ManagePosts.Controllers
{
    [Area("ManagePosts")]
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

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.ID_Account == user.Id && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 8 &&
               p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 7).ToList();

            return View(listpost);

        }

        public async Task<IActionResult> ListPost()
        {

            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.ID_Account == user.Id && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 8 &&
               p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 7).ToList();

            return View(listpost);

        }
        public async Task<IActionResult> ListHidePost()
        {

            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.ID_Account == user.Id && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 8).ToList();

            return View(listpost);

        }
       
      
        public async Task<IActionResult> ListFavoritePost()
        {

            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post_Favorite.Include(p => p.ID_PostNavigation).ThenInclude(p => p.PostTypeNavigation).Include(p => p.ID_PostNavigation).ThenInclude(p => p.RealEstateTypeNavigation).Include(p => p.ID_UserNavigation)
               .Where(p => p.ID_User == user.Id).ToList();
            return View(listpost);

        }

        // GET: Admin/ManagePostsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
                          .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
                          .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
                          .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
                          .Include(d => d.Post_Detail).
                          Include(image => image.Post_Image)
                          .Include(p => p.Post_Status)
                          .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.ID_Post == id).SingleOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            int Province = post.Post_Location.SingleOrDefault().Tinh_TPNavigation.id;
            int District = post.Post_Location.SingleOrDefault().Quan_HuyenNavigation.id;
            int Ward = post.Post_Location.SingleOrDefault().Phuong_XaNavigation.id;
            int Street = post.Post_Location.SingleOrDefault().Duong_PhoNavigation.id;
            ViewData["ID_Account"] = post.ID_Account;
            ViewData["ID_Account_Post"] = post.ID_Account == user.Id ? true : false;
            string nameuser = "";
            if (post.ID_AccountNavigation.IsAdmin == 1)
            {
                nameuser += "Admin " + _context.Admin.Where(p => p.Account_ID == post.ID_Account).SingleOrDefault().FullName;
            }
            else
            {
                var cus = _context.Customer.Where(p => p.Account_ID == post.ID_Account).SingleOrDefault();
                nameuser += cus.FirstName + " " + cus.LastName;
            }
            ViewData["Name_Account"] = nameuser;
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "_name", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description", post.RealEstateType);
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name", Province);
            ViewData["District"] = new SelectList(_context.district.OrderBy(p => p._name).Where(p => p._province_id == Province), "id", "_name", District);
            ViewData["Ward"] = new SelectList(_context.ward.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Ward);
            ViewData["Street"] = new SelectList(_context.street.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Street);

            return View(post);
        }

       

   
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
              .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
              .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
              .Include(d => d.Post_Detail).
              Include(image => image.Post_Image)
              .Include(p => p.Post_Status)
              .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.ID_Post == id).SingleOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (post.ID_Account != user.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            int Province = post.Post_Location.SingleOrDefault().Tinh_TPNavigation.id;
            int District = post.Post_Location.SingleOrDefault().Quan_HuyenNavigation.id;
            int Ward = post.Post_Location.SingleOrDefault().Phuong_XaNavigation.id;
            int Street = post.Post_Location.SingleOrDefault().Duong_PhoNavigation.id;
            ViewData["ID_Account"] = post.ID_Account;
            string nameuser = "";
            if (user.IsAdmin == 1)
            {
                nameuser = "Admin" + _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().FullName;
            }
            else
            {
                var cus = _context.Customer.Where(p => p.Account_ID == user.Id).SingleOrDefault();
                nameuser = cus.FirstName + " " + cus.LastName;
            }
            ViewData["Name_Account"] = nameuser;
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "_name", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description", post.RealEstateType);
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name", Province);
            ViewData["District"] = new SelectList(_context.district.OrderBy(p => p._name).Where(p => p._province_id == Province), "id", "_name", District);
            ViewData["Ward"] = new SelectList(_context.ward.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Ward);
            ViewData["Street"] = new SelectList(_context.street.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Street);


            return View(post);
        }

        // POST: Admin/ManagePostsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post, List<IFormFile> images, int district, int ward, int street
            , string diachi, bool alley, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket, string descriptiondetail, int bathroom,
            int bedroom, int yard, int floor, int province)
        {
            if (id != post.ID_Post)
            {
                return NotFound();
            }
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

            Post_Detail postdetail = _context.Post_Detail.Where(p => p.ID_Post == id).SingleOrDefault();
            postdetail.Alley = alley;
            postdetail.Bathroom = bathroom;
            postdetail.Bedroom = bedroom;
            postdetail.Description = descriptiondetail;
            postdetail.Floor = floor;
            postdetail.NearAirport = nearAirport;
            postdetail.NearHospital = nearHospital;
            postdetail.NearMarket = nearMarket;
            postdetail.NearSchool = nearSchool;
            postdetail.Yard = yard;
            Post_Location post_Location = _context.Post_Location.Where(p => p.ID_Post == id).SingleOrDefault();
            post_Location.DiaChi = diachi;
            post_Location.Duong_Pho = street;
            post_Location.Phuong_Xa = ward;
            post_Location.Quan_Huyen = district;
            post_Location.Tinh_TP = province;
            post_Location.DuAn = post.Project;
            var user = await _userManager.GetUserAsync(User);
            Post_Status post_Status = new Post_Status();
            post_Status.ID_Account = user.Id;
            post_Status.ID_Post = post.ID_Post;
            post_Status.Reason = "Bài đăng được cập nhật. Xin đợi admin duyệt";
            post_Status.Status = 5;
            post_Status.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    _context.Update(postdetail);
                    _context.Update(post_Location);
                    _context.Post_Status.Add(post_Status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID_Post))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return View(post);
                return RedirectToAction(nameof(Details), new { id = id });
            }

            int Province = post.Post_Location.SingleOrDefault().Tinh_TPNavigation.id;
            int District = post.Post_Location.SingleOrDefault().Quan_HuyenNavigation.id;
            int Ward = post.Post_Location.SingleOrDefault().Phuong_XaNavigation.id;
            int Street = post.Post_Location.SingleOrDefault().Duong_PhoNavigation.id;
            ViewData["ID_Account"] = user.Id;
            string nameuser = "";
            if (user.IsAdmin == 1)
            {
                nameuser = "Admin" + _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().FullName;
            }
            else
            {
                var cus = _context.Customer.Where(p => p.Account_ID == user.Id).SingleOrDefault();
                nameuser = cus.FirstName + " " + cus.LastName;
            }
            ViewData["Name_Account"] = nameuser;
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "_name", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description", post.RealEstateType);
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name", Province);
            ViewData["District"] = new SelectList(_context.district.OrderBy(p => p._name).Where(p => p._province_id == Province), "id", "_name", District);
            ViewData["Ward"] = new SelectList(_context.ward.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Ward);
            ViewData["Street"] = new SelectList(_context.street.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Street);
            return View(post);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int idpost, string reasonDeletePost)
        {

            var post = await _context.Post.FindAsync(idpost);
            if (post == null)
            {
                return NotFound();
            }
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = reasonDeletePost;
                poststatus.Status = 7;
                poststatus.ModifiedDate = DateTime.Now;
                _context.Post_Status.Add(poststatus);

            }
            try
            {

                await _context.SaveChangesAsync();
                post.Status = poststatus.ID_PostStatus;
                _context.Post.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                StatusMessage = "Xóa bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Xóa bài đăng không thành công";
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HidePost(int idpost)
        {

            var post = await _context.Post.FindAsync(idpost);
            if (post == null)
            {
                return NotFound();
            }
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Status = 8;
                poststatus.ModifiedDate = DateTime.Now;
                _context.Post_Status.Add(poststatus);

            }
            try
            {

                await _context.SaveChangesAsync();
                post.Status = poststatus.ID_PostStatus;
                _context.Post.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                StatusMessage = "Ẩn bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Ẩn bài đăng không thành công";
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public async Task<IActionResult> ShownPost(int idpost)
        {

            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = "";
                poststatus.Status = 5;
                poststatus.ModifiedDate = DateTime.Now;
                _context.Post_Status.Add(poststatus);

            }

            try
            {
                await _context.SaveChangesAsync();
                post.Status = poststatus.ID_PostStatus;
                _context.Post.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                var Pending = Int32.Parse(HttpContext.Session.GetString("PedingPost"))+1;
                HttpContext.Session.SetString("PedingPost", Pending.ToString());
                StatusMessage = "Hiện bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Hiện bài đăng không thành công";
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> SoldPost(int idpost)
        {

            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = "";
                poststatus.Status = 2;
                poststatus.ModifiedDate = DateTime.Now;
                _context.Post_Status.Add(poststatus);

            }

            try
            {
                await _context.SaveChangesAsync();
                post.Status = poststatus.ID_PostStatus;
                _context.Post.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                StatusMessage = "Đã chuyển bài đăng thành đã thanh toán thành công";
            }
            catch
            {
                StatusMessage = "Error Không thành công";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult DeleteFile(int id, int idimage)
        {
            string ID = id + "image" + idimage;
            try
            {
                var image = _context.Post_Image.Where(p => p.ID_Post == id && p.ID_Image == idimage).SingleOrDefault();
                if (image != null)
                {

                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\posts", image.url);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    _context.Post_Image.Remove(image);
                    _context.SaveChanges();
                    return Json(new { Result = "OK", id = ID });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "KHông tìm thấy ảnh" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }

        private bool PostExists(int iD_Post)
        {
            throw new NotImplementedException();
        }
    }
}