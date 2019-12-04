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
using BDS_ML.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;


namespace BDS_ML.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

        // GET: Dashboard
        public async Task<ActionResult> Index()
        {
           
            
            try
            {
              
              
                var list = _context.Post.Include(p => p.Post_Status);
                dboard.CustomerNumber = _context.Customer.Count();
                dboard.PostNumber = list.Count();
                if (HttpContext.Session.GetString("AvatarImage")==null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    HttpContext.Session.SetString("AvatarImage", _context.Admin.Where(p => p.Account_ID == user.Id).SingleOrDefault().Avatar_URL);
                  
                }
             
               
               
                int Pending = 0;
                int Sold = 0;
                int Posted = 0;
                foreach (var p in list)
                {
                    if (p.Post_Status.OrderBy(c=>c.ModifiedDate).LastOrDefault().Status == 5)
                        Pending++;
                    if (p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 2)
                        Sold++;
                    if (p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1)
                        Posted++;
                }
                dboard.PostPendingApprovalNumber = Pending;
                dboard.PostSoldNumber = Sold;
                dboard.PostedNumber = Posted;
                HttpContext.Session.SetString("PedingPost", Pending.ToString());
                StatusMessage = "Lấy dữ liệu thành công";
            }
            catch { StatusMessage = "Error Lấy dữ liệu không thành công"; }
            return View(dboard);
        }



    }
}