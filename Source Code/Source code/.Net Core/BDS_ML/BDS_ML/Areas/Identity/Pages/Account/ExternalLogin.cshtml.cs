using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace BDS_ML.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly BDT_MLDBContext _context;
        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = new BDT_MLDBContext();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email phải được điền.")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Họ phải được điền.")]
            [StringLength(50, ErrorMessage = "Họ phải dài hơn {2} kí tự.", MinimumLength = 2)]
            [Display(Name = "Họ")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Tên phải được điền.")]
            [StringLength(50, ErrorMessage = "Tên phải dài hơn {2} kí tự.", MinimumLength = 2)]
            [Display(Name = "Tên")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Số điện thoại phải được điền.")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
            [DataType(DataType.PhoneNumber, ErrorMessage = "Số điện thoại không hợp lệ.")]
            [StringLength(10, ErrorMessage = "Điện thoại chỉ chứa {2} kí tự số.", MinimumLength = 10)]
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Địa chỉ phải được điền.")]
            [StringLength(100, ErrorMessage = "Địa chỉ phải dài hơn {2} kí tự.", MinimumLength = 10)]
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            bool isExist = false;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

           
            var userID = _context.AspNetUserLogins.Where(p => p.ProviderKey == info.ProviderKey && p.LoginProvider == info.LoginProvider).SingleOrDefault().UserId;
            var user = _context.AspNetUsers.Where(p => p.Id == userID).SingleOrDefault();
            BDS_ML.Models.ModelDB.Admin admin = new Models.ModelDB.Admin();
            Customer cus = new Customer();
            if (user != null)
            {
                isExist = true;
                
                if (user.IsBlock != 0)
                {
                    if (user.IsAdmin == 0)
                    {
                        cus = _context.Customer.Where(c => c.Account_ID == user.Id).SingleOrDefault();
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
                            ErrorMessage = "Tài khoản bị khóa!. Lí do: " + block.Reason+".";
                            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                         
                        }
                    }
                    if (user.IsAdmin == 1)
                    {
                        admin = _context.Admin.Where(c => c.Account_ID == user.Id).SingleOrDefault();
                        var block = _context.Block.Where(b => b.ID_User == admin.ID_Admin).OrderBy(p => p.ModifiedDate).LastOrDefault();
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
                            ErrorMessage = "Tài khoản bị khóa!. Lí do: " + block.Reason + ".";
                            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                        }
                    }

                }

            }
           
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
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
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else if(isExist)
            {
                return RedirectToPage("./ExternalExistEmail");
            }
            else 
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {

                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        Address = info.Principal.FindFirstValue(ClaimTypes.Country),
                        PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        Customer customer = new Customer();

                        customer.Avatar_URL = "avatar_common.png";

                        customer.Account_ID = user.Id;
                        customer.FirstName = Input.FirstName;
                        customer.LastName = Input.LastName;
                        customer.PhoneNumber = Input.PhoneNumber;
                        customer.Email = Input.Email;
                        customer.Address = Input.Address;
                        customer.ModifiedDate = DateTime.Now;
                        customer.CreatedDate = DateTime.Now;
                        try
                        {

                            _context.Customer.Add(customer);
                            _context.SaveChanges();
                            user.EmailConfirmed = true;
                            var updateResult = await _userManager.UpdateAsync(user);
                            if (!updateResult.Succeeded)
                            {

                                var userId = await _userManager.GetUserIdAsync(user);
                                throw new InvalidOperationException($"Unexpected error occurred setting fields for user with ID '{userId}'.");
                            }
                            _logger.LogInformation("Created a new customer with account.");
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation("Error a new customer with account." + "Error: " + e);
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return LocalRedirect(returnUrl);

                    }

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
