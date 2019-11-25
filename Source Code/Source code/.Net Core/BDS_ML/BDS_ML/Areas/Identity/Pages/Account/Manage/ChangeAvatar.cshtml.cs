using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDS_ML.Areas.Identity.Pages.Account.Manage
{
    public class ChangeAvatarModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BDT_MLDBContext _context;

        public ChangeAvatarModel(
            UserManager<ApplicationUser> userManager
          )
        {
            _userManager = userManager;

            _context = new BDT_MLDBContext();
        }
        [TempData]
        public string StatusMessage { get; set; }
        public string ReturnUrl { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            

            
            public string AvatarImage { get; set; }
          
            public IFormFile AvatarImageFile { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var id = await _userManager.GetUserIdAsync(user);
            
            if(user.IsAdmin==1)
            {
                BDS_ML.Models.ModelDB.Admin admin = _context.Admin.Where(c => c.Account_ID == id).SingleOrDefault();


                Input = new InputModel
                {
                    AvatarImage = admin.Avatar_URL

                };
            }
            if(user.IsAdmin==0)
            {
                Customer customer = _context.Customer.Where(c => c.Account_ID == id).SingleOrDefault();


                Input = new InputModel
                {
                    AvatarImage = customer.Avatar_URL

                };
            }
            return Page();
        }
   
        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if(user.IsAdmin==1)
            {
                BDS_ML.Models.ModelDB.Admin admin = _context.Admin.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                var email = await _userManager.GetEmailAsync(user);
                if (admin == null)
                {
                    return NotFound($"Unable to load admin with úuerID '{_userManager.GetUserId(User)}'.");
                }

                if (image != null)
                {

                    string fileName = Path.GetFileName(image.FileName);

                    string extensionFileName = Path.GetExtension(fileName);

                    fileName = fileName.Substring(0, fileName.Length - extensionFileName.Length) + "-" + user.Id + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\avatars", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    admin.Avatar_URL = fileName;

                }
                else
                    admin.Avatar_URL = "avatar_common.png";

                {
                    try
                    {
                        _context.Admin.Attach(admin);
                        _context.Entry(admin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                    }
                    catch
                    {
                        StatusMessage = "Error Cập nhật thông tin không thành công!";
                        return RedirectToPage();
                    }
                }
            }
            
           if(user.IsAdmin==0)
            {
                Customer customer = _context.Customer.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                var email = await _userManager.GetEmailAsync(user);
                if (customer == null)
                {
                    return NotFound($"Unable to load admin with úuerID '{_userManager.GetUserId(User)}'.");
                }

                if (image != null)
                {

                    string fileName = Path.GetFileName(image.FileName);

                    string extensionFileName = Path.GetExtension(fileName);

                    fileName = fileName.Substring(0, fileName.Length - extensionFileName.Length) + "-" + user.Id + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\avatars", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    customer.Avatar_URL = fileName;

                }
                else
                    customer.Avatar_URL = "avatar_common.png";

                {
                    try
                    {
                        _context.Customer.Attach(customer);
                        _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                    }
                    catch
                    {
                        StatusMessage = "Error Cập nhật thông tin không thành công!";
                        return RedirectToPage();
                    }
                }
            }
            StatusMessage = "Thông tin của bạn đã được cập nhật";
            return RedirectToPage();
        }
    }
}