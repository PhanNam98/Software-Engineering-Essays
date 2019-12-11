using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BDS_ML.Models.Security;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Http;

namespace BDS_ML.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly BDT_MLDBContext _context;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = new BDT_MLDBContext();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email phải được điền.")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu phải được điền.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = _context.AspNetUsers.Where(p => p.UserName == Input.Email).SingleOrDefault();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập không thành công!.");
                return Page();
            }
            BDS_ML.Models.ModelDB.Admin admin=new Models.ModelDB.Admin();
            Customer cus=new Customer();
           
            if (user.IsBlock != 0)
            {
                if (user.IsAdmin == 0)
                {
                    //var cus = _context.Customer.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                    var block = _context.Block.Where(b => b.ID_User == cus.ID_User).OrderBy(p => p.ModifiedDate).LastOrDefault();
                    if (block.UnLockDate <= DateTime.Now)
                    {
                        try
                        {
                            block.ModifiedDate = DateTime.Now.Date;
                            user.IsBlock = 0;
                            _context.AspNetUsers.Attach(user);
                            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            _context.Block.Attach(block);
                            _context.Entry(block).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();

                        }
                        catch { }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản bị khóa!. Lí do: " + block.Reason);
                        return Page();
                    }
                }
                if (user.IsAdmin == 1)
                {
                    //admin = _context.Admin.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                    var block = _context.Block.Where(b => b.ID_User == admin.ID_Admin).OrderBy(p=>p.ModifiedDate).LastOrDefault();
                    if (block.UnLockDate <= DateTime.Now)
                    {
                        try
                        {
                            block.ModifiedDate = DateTime.Now.Date;
                            user.IsBlock = 0;
                            _context.AspNetUsers.Attach(user);
                            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            _context.Block.Attach(block);
                            _context.Entry(block).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();

                        }
                        catch { }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản bị khóa!. Lí do: " + block.Reason);
                        return Page();
                    }
                }

            }
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    string urlavatar = "";
                    if (user.IsAdmin == 1)
                    {
                        admin = _context.Admin.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                        urlavatar += admin.Avatar_URL;
                    }
                    else
                    {
                        cus = _context.Customer.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                        urlavatar += cus.Avatar_URL;
                    }
                    HttpContext.Session.SetString("AvatarImage", urlavatar);
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công!.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
