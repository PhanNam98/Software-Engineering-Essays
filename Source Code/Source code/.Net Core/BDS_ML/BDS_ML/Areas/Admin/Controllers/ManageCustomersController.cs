using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;

namespace BDS_ML.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageCustomersController : Controller
    {
        private readonly BDT_MLDBContext _context;
        [TempData]
        public string StatusMessage { get; set; }
        public ManageCustomersController()
        {
            _context = new BDT_MLDBContext();
        }

        // GET: Admin/ManageCustomers
        public async Task<IActionResult> Index()
        {

            var bDT_MLDBContext = _context.Customer.Include(c => c.Account_);
            if (bDT_MLDBContext.ToListAsync().Result.Count() != 0)
                StatusMessage = "Lấy danh sách thành công";
            else
            {
                StatusMessage = "Error Lấy danh sách không thành công";
            }
            return View(await bDT_MLDBContext.ToListAsync());




        }

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
    }
}
