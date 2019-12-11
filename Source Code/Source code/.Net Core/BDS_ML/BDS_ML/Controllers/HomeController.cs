using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BDS_ML.Models;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models.Data;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BDS_ML.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class HomeController : Controller
    {

        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _context = new BDT_MLDBContext();
            _userManager = userManager;
          
        }
        public async Task<IActionResult> Index()
        {
            Home_Index data = new Home_Index();
            PostIndex postIndex = new PostIndex();

            int count_canban = 0;
            int count_canmua = 0;
            int count_canthue = 0;
            int count_canchothue = 0;

            data.lst3HotPosts = postIndex.get3HotPosts();
            data.lst6PopularPosts = postIndex.get6PopularPosts();

            count_canban = postIndex.getCount(1);
            count_canmua = postIndex.getCount(3);
            count_canthue = postIndex.getCount(4);
            count_canchothue = postIndex.getCount(2);

            ViewBag.canban = count_canban;
            ViewBag.canmua = count_canmua;
            ViewBag.canthue = count_canthue;
            ViewBag.canchothue = count_canchothue;
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                ViewBag.favoritepost= _context.Post_Favorite.Include(p=>p.ID_PostNavigation).ThenInclude(p=>p.Post_Image).Include(p => p.ID_PostNavigation).ThenInclude(p => p.PostTypeNavigation).Include(p => p.ID_PostNavigation).ThenInclude(p => p.RealEstateTypeNavigation).Include(p => p.ID_UserNavigation)
               .Where(p => p.ID_User == user.Id).OrderByDescending(p=>p.MortifiedDate).Take(5).ToList();
            }
            return View(data);
        }
        
        //[Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Join()
        {
            return View();
        }
        public IActionResult Advertisement()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
