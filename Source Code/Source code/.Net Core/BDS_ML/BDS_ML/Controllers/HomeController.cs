using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BDS_ML.Models;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models.Data;

namespace BDS_ML.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Home_Index data = new Home_Index();
            PostIndex postIndex = new PostIndex();

            data.lst3HotPosts = postIndex.get3HotPosts();
            data.lst6PopularPosts = postIndex.get6PopularPosts();

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
