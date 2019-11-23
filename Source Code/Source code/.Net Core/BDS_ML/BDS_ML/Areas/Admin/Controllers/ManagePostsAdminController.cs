using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models.CustomModel;
using Microsoft.AspNetCore.Identity;
using BDS_ML.Models;

namespace BDS_ML.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManagePostsAdminController : Controller
    {
        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManagePostsAdminController(UserManager<ApplicationUser> userManager)
        {
            _context = new BDT_MLDBContext();
            _userManager = userManager;
            StatusMessage = "Đang xử lí";
        }
        [TempData]
        public string StatusMessage { get; set; }
        // GET: Admin/ManagePostsAdmin
        public async Task<IActionResult> Index()
        {
            //var bDT_MLDBContext = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
            //  .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Status).OrderByDescending(p => p.PostTime);
            //return View(await bDT_MLDBContext.ToListAsync());
            
           var listpost =await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation)
              .Include(p => p.Post_Status)
              .ThenInclude(post=>post.StatusNavigation.Post_Status).ToListAsync();
            //var listStatuspost =await _context.Status.ToArrayAsync();
            //var postRecord = from p in listpost
            //                     join s in listStatuspost on p.Post_Status.LastOrDefault().Status equals s.ID_Status 
            //                    where p.Post_Status.LastOrDefault().Status != 6
            //                     select new PostCustom
            //                     {
            //                        post = p,
            //                         statusPost = s.Description,
                                    
            //                     };
            //var a = listpost.Where(p=>p.ID_Post==2).SingleOrDefault().Post_Status.OrderBy(p=>p.ModifiedDate).ToList();
            return View(listpost);

        }
        public async Task<IActionResult> ListPost()
        {
          
            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p=>p.ID_Account==user.Id).ToList();
            List<Post> a = listpost;
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
            ViewData["ID_Account_Post"] = post.ID_Account==user.Id?true:false;
            string nameuser = "";
            if (user.IsAdmin == 1)
            {
                nameuser += "Admin " + _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().FullName;
            }
            else
            {
                var cus = _context.Customer.Where(p => p.Account_ID == user.Id).SingleOrDefault();
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

        // GET: Admin/ManagePostsAdmin/Create
        public IActionResult Create()
        {
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType");
            ViewData["Project"] = new SelectList(_context.project, "id", "id");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType");
            return View();
        }

        // POST: Admin/ManagePostsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.ID_Account);
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "id", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType", post.RealEstateType);
            return View(post);
        }

        // GET: Admin/ManagePostsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation).Include(p=>p.Post_Location).ThenInclude(lo=>lo.Tinh_TPNavigation.Post_Location)
              .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
              .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
              .Include(d=>d.Post_Detail).
              Include(image=>image.Post_Image)
              .Include(p => p.Post_Status)
              .ThenInclude(pt=> pt.StatusNavigation.Post_Status).Where(p=>p.ID_Post==id).SingleOrDefaultAsync();
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
            string nameuser = "";
            if(user.IsAdmin==1)
            {
                nameuser ="Admin"+ _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().FullName;
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
            ViewData["District"] = new SelectList(_context.district.OrderBy(p => p._name).Where(p=>p._province_id== Province), "id", "_name", District);
            ViewData["Ward"] = new SelectList(_context.ward.OrderBy(p => p._name).Where(p=>p._province_id==Province && p._district_id==District), "id", "_name", Ward);
            ViewData["Street"] = new SelectList(_context.street.OrderBy(p => p._name).Where(p => p._province_id == Province && p._district_id == District), "id", "_name", Street);


            return View(post);
        }

        // POST: Admin/ManagePostsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post, int district, int ward, int street
            , string diachi, bool alley, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket, string descriptiondetail, int bathroom,
            int bedroom, int yard, int floor, int province)
        {
            if (id != post.ID_Post)
            {
                return NotFound();
            }
            Post_Detail postdetail =_context.Post_Detail.Where(p=>p.ID_Post==id).SingleOrDefault();
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
            Post_Location post_Location = _context.Post_Location.Where(p=>p.ID_Post==id).SingleOrDefault();
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
                return RedirectToAction(nameof(Details),new { id=id});
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

        // GET: Admin/ManagePostsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.ID_AccountNavigation)
                .Include(p => p.PostTypeNavigation)
                .Include(p => p.ProjectNavigation)
                .Include(p => p.RealEstateTypeNavigation)
                .FirstOrDefaultAsync(m => m.ID_Post == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/ManagePostsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID_Post == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockPost(int idpost,string reasonBlockPost)
        {
            if (reasonBlockPost ==null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post!=null)
            {
                
                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account=user.Id;
                poststatus.Reason = reasonBlockPost;
                poststatus.Status = 3;
                poststatus.ModifiedDate = DateTime.Now;
                _context.Post_Status.Add(poststatus);
                
            }
            try {
               
                await _context.SaveChangesAsync();
                post.Status = poststatus.ID_PostStatus;
                _context.Post.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                StatusMessage = "Khóa bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Khóa bài đăng không thành công";
            }
            
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UnBlockPost(int idpost)
        {
           
            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {
               
                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = "";
                poststatus.Status = 1;
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
                StatusMessage = "Mở khóa bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Mở khóa bài đăng không thành công";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPost(int idpost)
        {

            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = "";
                poststatus.Status = 1;
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
                StatusMessage = "Duyệt bài đăng thành công";
            }
            catch
            {
                StatusMessage = "Error Duyệt bài đăng không thành công";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> IgnorePost(int idpost)
        {

            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = "";
                poststatus.Status = 6;
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
                StatusMessage = "Đã ẩn bài đăng không được duyệt thành công";
            }
            catch
            {
                StatusMessage = "Error Ẩn bài đăng không thành công";
            }
            return RedirectToAction(nameof(Index));
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
