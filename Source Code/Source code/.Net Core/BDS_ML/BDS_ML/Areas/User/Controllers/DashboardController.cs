using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Mvc;

namespace BDS_ML.Areas.User.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BDT_MLDBContext _context;
        [TempData]
        public string StatusMessage { get; set; }
        public DashboardController()
        {
            _context = new BDT_MLDBContext();
            StatusMessage = "Đang xử lí";


        }
        public IActionResult Index()
        {
            return View();
        }
    }
}