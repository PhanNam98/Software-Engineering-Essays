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

namespace BDS_ML.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManagePostsAdminController : Controller
    {
        private readonly BDT_MLDBContext _context;

        public ManagePostsAdminController()
        {
            _context = new BDT_MLDBContext();
        }

        // GET: Admin/ManagePostsAdmin
        public IActionResult Index()
        {
            //var bDT_MLDBContext = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
            //  .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Status).OrderByDescending(p => p.PostTime);
            //return View(await bDT_MLDBContext.ToListAsync());
            
           var listpost = _context.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
              .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Status).OrderByDescending(p => p.PostTime).ToList();
            var listStatuspost = _context.Status.ToList();
            var postRecord = from p in listpost
                                 join s in listStatuspost on p.Post_Status.LastOrDefault().Status equals s.ID_Status 
                                
                                 select new PostCustom
                                 {
                                    post = p,
                                     statusPost = s.Description,
                                    
                                 };
            return View(postRecord);

        }

        // GET: Admin/ManagePostsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.ID_AccountNavigation)
                .Include(p => p.PostTypeNavigation)
                .Include(p => p.ProjectNavigation)
                .Include(p => p.RealEstateTypeNavigation)
                .FirstOrDefaultAsync(m => m.ID_Post == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/ManagePostsAdmin/Create
        public IActionResult Create()
        {
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType");
            ViewData["Project"] = new SelectList(_context.project, "id", "id");
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType");
            return View();
        }

        // POST: Admin/ManagePostsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.ID_Account);
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "id", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType", post.RealEstateType);
            return View(post);
        }

        // GET: Admin/ManagePostsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.ID_Account);
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "id", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType", post.RealEstateType);
            return View(post);
        }

        // POST: Admin/ManagePostsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Post,ID_Account,PostTime,PostType,Tittle,Size,Project,Price,RealEstateType,Description,Status")] Post post)
        {
            if (id != post.ID_Post)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID_Post))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Account"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.ID_Account);
            ViewData["PostType"] = new SelectList(_context.Post_Type, "ID_PostType", "ID_PostType", post.PostType);
            ViewData["Project"] = new SelectList(_context.project, "id", "id", post.Project);
            ViewData["RealEstateType"] = new SelectList(_context.RealEstate_Type, "ID_RealEstateType", "ID_RealEstateType", post.RealEstateType);
            return View(post);
        }

        // GET: Admin/ManagePostsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.ID_AccountNavigation)
                .Include(p => p.PostTypeNavigation)
                .Include(p => p.ProjectNavigation)
                .Include(p => p.RealEstateTypeNavigation)
                .FirstOrDefaultAsync(m => m.ID_Post == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/ManagePostsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID_Post == id);
        }
    }
}
