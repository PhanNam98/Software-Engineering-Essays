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
using Microsoft.AspNetCore.Http;

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
                if (HttpContext.Session.GetString("AvatarImage") == null)
                {
                    
                    HttpContext.Session.SetString("AvatarImage", _context.Customer.Where(p => p.Account_ID == user.Id).SingleOrDefault().Avatar_URL);

                }
                var list = _context.Post.Include(p => p.Post_Status).Where(p => p.ID_Account == user.Id);
                int Pending = 0;
                int Sold = 0;
                foreach (var p in list)
                {
                    if (p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 5)
                        Pending++;
                    if (p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 2)
                        Sold++;
                }
                dboard.PostPending = Pending;
                dboard.PostNumber = list.Count();
                dboard.PostSoldNumber = Sold;
                dboard.Postfollowed = _context.Post_Favorite.Where(c => c.ID_User == user.Id).Count();
                StatusMessage = "Lấy dữ liệu thành công";
            }
            catch { StatusMessage = "Error Lấy dữ liệu không thành công"; }
            return View(dboard);
        }
    }
}