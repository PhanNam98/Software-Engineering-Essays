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



        }

        // GET: Dashboard
        public ActionResult Index()
        {

            try
            {
                var list = _context.Post.Include(p => p.Post_Status);
                dboard.CustomerNumber = _context.Customer.Count();
                dboard.PostNumber = list.Count();


                int Pending = 0;
                int Sold = 0;
                foreach (var p in list)
                {
                    if (p.Post_Status.LastOrDefault().Status == 5)
                        Pending++;
                    if (p.Post_Status.LastOrDefault().Status == 2)
                        Sold++;
                }
                dboard.PostPendingApprovalNumber = Pending;
                dboard.PostSoldNumber = Sold;


                StatusMessage = "Lấy dữ liệu thành công";
            }
            catch { StatusMessage = "Error Lấy dữ liệu không thành công"; }
            return View(dboard);
        }



    }
}