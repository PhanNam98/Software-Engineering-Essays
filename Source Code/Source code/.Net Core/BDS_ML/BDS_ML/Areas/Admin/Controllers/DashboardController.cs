using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Areas.Admin.Models;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDS_ML.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly BDT_MLDBContext _context;
        [TempData]
        public string StatusMessage { get; set; }
       
        public Dashboard dboard = new Dashboard();
        public DashboardController()
        {
            _context = new BDT_MLDBContext();
            StatusMessage = "Đang xử lí";
           
           
        }

        // GET: Dashboard
        public ActionResult Index()
        {
           
            try
            {
                dboard.CustomerNumber = _context.Customer.Count();
                dboard.PostNumber = _context.Post.Count();
                dboard.PostSoldNumber = _context.Post_Status.Where(c => c.Status == 2).Count();
                dboard.PostPendingApprovalNumber = _context.Post_Status.Where(c => c.Status == 5).Count();
                StatusMessage = "Lấy dữ liệu thành công";
            }
            catch { StatusMessage = "Error Lấy dữ liệu không thành công"; }
            return View(dboard);
        }

       
      
    }
}