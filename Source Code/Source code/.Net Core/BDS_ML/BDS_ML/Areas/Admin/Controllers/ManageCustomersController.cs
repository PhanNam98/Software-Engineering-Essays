using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models;
using BDS_ML.Models.CustomModel;
using Microsoft.AspNetCore.Identity;

namespace BDS_ML.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageCustomersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BDT_MLDBContext _context;
        [TempData]
        public string StatusMessage { get; set; }

        public ManageCustomersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = new BDT_MLDBContext();
            StatusMessage = "Đang xử lí";

        }

        //GET: Admin/ManageCustomers
        public async Task<IActionResult> Index()
        {
            DateTime date = DateTime.Now.AddDays(1);
            if (date.Day < 10)
                ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-0" + date.Day.ToString();
            else
                ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
            string a = ViewData["DateNow"].ToString();

            var bDT_MLDBContext = _context.Customer.Include(c => c.Account_).Include(p => p.Block).Where(c => c.Account_.IsAdmin == 0);
            if (bDT_MLDBContext.ToListAsync().Result.Count() != 0)
                StatusMessage = "Lấy danh sách thành công";
            else
            {
                StatusMessage = "Error Lấy danh sách không thành công";
            }
            return View(await bDT_MLDBContext.ToListAsync());

        }
        //Code Test
        //public IActionResult Index()
        //{
        //    DateTime date = DateTime.Now.AddDays(1);
        //    if (date.Day < 10)
        //        ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-0" + date.Day.ToString();
        //    else
        //        ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
        //    string a = ViewData["DateNow"].ToString();

        //    var listCus = _context.Customer.Include(c => c.Account_).Include(p => p.Block).Where(c => c.Account_.IsAdmin == 0).ToList();

        //    if (listCus.Count() != 0)
        //    {
        //        var listAdmin = _context.Admin.ToList();
        //        var cusRecord = from c in listCus
        //                       join ad in listAdmin on c.Block.LastOrDefault().ID_Admin equals ad.ID_Admin

        //                         select new CustomerCustom
        //                         {
        //                             customer = c,
        //                             nameAdmin = ad.FullName,

        //                         };
        //        StatusMessage = "Lấy danh sách thành công";
        //        return View(cusRecord);
        //    }
        //    else
        //    {
        //        StatusMessage = "Error Lấy danh sách không thành công";
        //    }
        //    return View(null);

        //}

        // GET: Admin/ManageCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.Account_)
                .FirstOrDefaultAsync(m => m.ID_User == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        //Không cần thiết
        // GET: Admin/ManageCustomers/Create
        //public IActionResult Create()
        //{
        //    ViewData["Account_ID"] = new SelectList(_context.AspNetUsers, "Id", "Id");
        //    return View();
        //}

        // POST: Admin/ManageCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID_User,FirstName,LastName,Email,PhoneNumber,Address,Account_ID,Avatar_URL,ModifiedDate,CreatedDate")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Account_ID"] = new SelectList(_context.AspNetUsers, "Id", "Id", customer.Account_ID);
        //    return View(customer);
        //}

        // GET: Admin/ManageCustomers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customer.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Account_ID"] = new SelectList(_context.AspNetUsers, "Id", "Id", customer.Account_ID);
        //    return View(customer);
        //}

        // POST: Admin/ManageCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID_User,FirstName,LastName,Email,PhoneNumber,Address,Account_ID,Avatar_URL,ModifiedDate,CreatedDate")] Customer customer)
        //{
        //    if (id != customer.ID_User)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(customer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CustomerExists(customer.ID_User))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Account_ID"] = new SelectList(_context.AspNetUsers, "Id", "Id", customer.Account_ID);
        //    return View(customer);
        //}

        // GET: Admin/ManageCustomers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customer
        //        .Include(c => c.Account_)
        //        .FirstOrDefaultAsync(m => m.ID_User == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        // POST: Admin/ManageCustomers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customer.FindAsync(id);
        //    _context.Customer.Remove(customer);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.ID_User == id);
        }
        [HttpPost]
        public async Task<IActionResult> BlockCustomer(int iDCus, string reasonBlockCus, DateTime? dateUnBlock, string id_acc)
        {
            var admin = await _userManager.GetUserAsync(User);
            if (reasonBlockCus != null && reasonBlockCus != "")
            {
                Block block = new Block();
                block.ID_User = iDCus;
                block.Reason = reasonBlockCus;
                block.UnLockDate = dateUnBlock;
                block.BlockDate = DateTime.Now;
                block.ModifiedDate = DateTime.Now;
                try
                {
                    block.ID_Admin = _context.Admin.Where(q => q.Account_ID == admin.Id).SingleOrDefault().ID_Admin;
                }
                catch
                {
                    StatusMessage = "Error Không tìm thấy id admin thích hợp";
                }
                _context.Block.Add(block);
              

                var user = await _userManager.FindByIdAsync(id_acc);
                AspNetUsers USer = _context.AspNetUsers.Where(p=>p.Id==id_acc).SingleOrDefault();
                if (user == null && USer==null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                
                user.IsBlock = 1;
                USer.IsBlock = 1;
                _context.AspNetUsers.Attach(USer);
                _context.Entry(USer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var a = user;
                    StatusMessage = "Error Khóa không thành công";
                    return RedirectToAction("Index", "ManageCustomers");
                    //var userId = await _userManager.GetUserIdAsync(user);
                    //throw new InvalidOperationException($"Unexpected error occurred setting fields for user with ID '{userId}'.");
                }
               
                
                _context.Block.Add(block);
                _context.SaveChanges();
                StatusMessage = "Khóa thành công";
                return RedirectToAction("Index", "ManageCustomers");
            }
            else
                StatusMessage = "Error Khóa không có lí do";

            return RedirectToAction("Index", "ManageCustomers");

        }
        public async Task<IActionResult> BlockList()
        {
            try
            {
                var list = await _context.Block.Include(c=>c.ID_AdminNavigation).Include(c=>c.ID_UserNavigation).Include(p=>p.ID_UserNavigation.Account_).OrderByDescending(p=>p.ID_Block).ThenByDescending(p=>p.ID_Block).ToListAsync();
                StatusMessage = "Lấy danh sách thành công";
                return View(list);
            }
            catch { StatusMessage = "Error Lấy danh sách không thành công"; }
            return View();
        }
        public async Task<IActionResult> BlockListAccount()
        {
            try
            {
                DateTime date = DateTime.Now.AddDays(1);
                if (date.Day < 10)
                    ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-0" + date.Day.ToString();
                else
                    ViewData["DateNow"] = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
                string a = ViewData["DateNow"].ToString();
                var list = await _context.Customer.Include(p=>p.Block).Include(p=>p.Account_).Where(p=>p.Account_.IsAdmin==0 && p.Account_.IsBlock!=0).OrderByDescending(p => p.ID_User).ToListAsync();
                StatusMessage = "Lấy danh sách thành công";
                return View(list);
            }
            catch { StatusMessage = "Error Lấy danh sách không thành công"; }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UnBlockCustomer(int idcus,string idacc, int idblock,string returnURL=null)
        {
              string returnUrl = returnURL ?? Url.Content("~/Admin/BlockList");
            try
            {
               
                var block = await _context.Block.Where(p => p.ID_User == idcus && p.ID_Block==idblock).SingleOrDefaultAsync();
                if(block==null)
                {
                    StatusMessage = "Error Không tìm thấy khách hàng";
                    return RedirectToAction("Index", "ManageCustomers");
                }
                var admin = await _userManager.GetUserAsync(User);
                if (block.ID_Admin!= _context.Admin.Where(q => q.Account_ID == admin.Id).SingleOrDefault().ID_Admin)
                {
                    StatusMessage = "Error Tài khoản Admin không có quyền mở khóa khách hàng này";
                    return RedirectToAction("Index", "ManageCustomers");
                }
                //block.UnLockDate = DateTime.Now.Date;
                block.ModifiedDate = DateTime.Now.Date;
                var user = await _userManager.FindByIdAsync(idacc);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                user.IsBlock = 0;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                   
                    StatusMessage = "Error Mở khóa không thành công";
                    return RedirectToAction("BlockList", "ManageCustomers");
                    //var userId = await _userManager.GetUserIdAsync(user);
                    //throw new InvalidOperationException($"Unexpected error occurred setting fields for user with ID '{userId}'.");
                }
                _context.Block.Attach(block);
                _context.Entry(block).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch { }
            StatusMessage = "Mở khóa thành công";
            return LocalRedirect(returnUrl);
            //return RedirectToAction("BlockList", "ManageCustomers");

        }
    }
}
