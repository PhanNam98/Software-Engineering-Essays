using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDS_ML.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly BDT_MLDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(UserManager<ApplicationUser> userManager)
        {
            _context = new BDT_MLDBContext();
            _userManager = userManager;
            StatusMessage = "Đang xử lí";
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user==null)
            {
                return LocalRedirect("~/Identity/Account/Login?returnUrl=~/post/create");
            }
            ViewData["PostType"] = new SelectList(_context.Post_Type,"ID_PostType", "Description");
            ViewData["Project"] = new SelectList(_context.project, "id", "_name");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type , "ID_RealEstateType", "Description");
            ViewData["IDAccount"] = user.Id;
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p=>p._name), "id", "_name");
            return View();
        }

        // POST: Admin/ManagePostsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post,string id
            ,int district,int ward,bool alley,bool nearSchool,bool nearAirport,bool nearHospital,bool nearMarket, int province)
        {
            
            var user = await _userManager.GetUserAsync(User);
            post.PostTime = DateTime.Now;
            post.ID_Account = user.Id;
            if (ModelState.IsValid)
            {
                string a = post.Tittle;
                _context.Add(post);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index","ManagePostsAdmin", new { area = "Admin" });
            }
          
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "Description");
            ViewData["Project"] = new SelectList(_context.project, "id", "_name");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "Description");
            ViewData["IDAccount"] = user.Id;
            ViewData["Province"] = new SelectList(_context.province.OrderBy(p => p._name), "id", "_name");
            return View(post);
        }
        public JsonResult Get_district(int province_id)
        {
            var list =_context.district.Where(p=>p._province_id== province_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name =x._prefix+" "+ x._name
            }).ToList());
        }
        public JsonResult Get_ward(int province_id,int district_id)
        {
            var list = _context.ward.Where(p => p._province_id == province_id && p._district_id==district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name =x._prefix+" "+x._name
            }).ToList());
        }
        public JsonResult Get_street(int province_id, int district_id)
        {
            var list = _context.street.Where(p => p._province_id == province_id && p._district_id == district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name =x._prefix+" "+ x._name
            }).ToList());
        }


    }
}