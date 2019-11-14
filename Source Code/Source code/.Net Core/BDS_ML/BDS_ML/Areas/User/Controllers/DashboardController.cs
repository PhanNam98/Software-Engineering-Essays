using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Mvc;
using BDS_ML.Areas.User.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BDS_ML.Models;
using Microsoft.EntityFrameworkCore;

namespace BDS_ML.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class DashboardController : Controller
    {
        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        [TempData]
        public string StatusMessage { get; set; }
        public Dashboard dboard = new Dashboard();
        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _context = new BDT_MLDBContext();
            StatusMessage = "Đang xử lí";
            _userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            
            try
            {
                var user = await _userManager.GetUserAsync(User);
                
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                
                dboard.PostPending = _context.Post_Status.Include(p=>p.ID_PostStatus).Where(p=>p.ID_Account==user.Id && p.Status==5).Count();
                dboard.PostNumber = _context.Post.Where(p=>p.ID_Account==user.Id).Count();
                dboard.PostSoldNumber = _context.Post_Status.Include(p => p.ID_PostStatus).Where(p => p.ID_Account == user.Id && p.Status == 2).Count();
                dboard.Postfollowed = _context.Post_Favorite.Where(c => c.ID_User == user.Id).Count();
                StatusMessage = "Lấy dữ liệu thành công";
            }
            catch { StatusMessage = "Error Lấy dữ liệu không thành công"; }
            return View(dboard);
        }
    }
}