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
using Microsoft.AspNetCore.Http;
using System.IO;

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
              .ThenInclude(post=>post.StatusNavigation.Post_Status).Where(p=> p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 7
              && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 6
              && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 2
              && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 8).ToListAsync();
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
        //public async Task<IActionResult> ListPost()
        //{

        //    var user = await _userManager.GetUserAsync(User);
        //    var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
        //       .Include(p => p.RealEstateTypeNavigation)
        //       .Include(p => p.Post_Status)
        //       .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p=>p.ID_Account==user.Id && p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 8 &&
        //       p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status != 7).ToList();

        //    return View(listpost);

        //}



        //public async Task<IActionResult> ListHidePost()
        //{

        //    var user = await _userManager.GetUserAsync(User);
        //    var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
        //       .Include(p => p.RealEstateTypeNavigation)
        //       .Include(p => p.Post_Status)
        //       .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.ID_Account == user.Id && p.Post_Status.OrderBy(c=>c.ModifiedDate).LastOrDefault().Status==8).ToList();

        //    return View(listpost);

        //}
        public async Task<IActionResult> ListPedingPost()
        {

            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 5).ToList();

            return View(listpost);

        }
        public async Task<IActionResult> ListSoldPost()
        {

            var user = await _userManager.GetUserAsync(User);
            var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
               .Include(p => p.RealEstateTypeNavigation)
               .Include(p => p.Post_Status)
               .ThenInclude(post => post.StatusNavigation.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 2).ToList();

            return View(listpost);

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
        public async Task<IActionResult> IgnorePost(int idpost,string reasonIgnorePost)
        {

            var post = await _context.Post.FindAsync(idpost);
            Post_Status poststatus = new Post_Status();
            if (post != null)
            {

                poststatus.ID_Post = post.ID_Post;
                var user = await _userManager.GetUserAsync(User);
                poststatus.ID_Account = user.Id;
                poststatus.Reason = reasonIgnorePost;
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
       
       
        [HttpPost]
        public JsonResult DeleteFile(int id,int idimage)
        {
            string ID = id + "image" + idimage;
            try
            {
               var image= _context.Post_Image.Where(p => p.ID_Post == id && p.ID_Image == idimage).SingleOrDefault();
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
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
               
           
        }
    }

}
