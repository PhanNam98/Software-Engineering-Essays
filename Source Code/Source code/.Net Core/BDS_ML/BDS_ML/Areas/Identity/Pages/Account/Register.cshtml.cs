using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BDS_ML.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BDS_ML.Models.Mail;
using BDS_ML.Models.Security;
using BDS_ML.Models.ModelDB;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BDS_ML.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly BDT_MLDBContext _context;
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = new BDT_MLDBContext();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(50, ErrorMessage = "The First Name must be at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [StringLength(50, ErrorMessage = "The Last Name must be at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [StringLength(15, ErrorMessage = "The Phone Number must be at max {1} characters long.", MinimumLength = 10)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "the address must be at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile Avatar_URL { get; set; }
            
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile image,string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
              
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email,
                PhoneNumber=Input.PhoneNumber};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User created a new account with password.");
                    Customer customer = new Customer();
                    if (image != null)
                    {
                        string fileName = Path.GetFileName(image.FileName);

                        string extensionFileName = Path.GetExtension(fileName);

                        fileName = fileName.Substring(0, fileName.Length - extensionFileName.Length) + "-" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + extensionFileName;

                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\avatars", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        customer.Avatar_URL = fileName;
                        
                    }
                    
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
                        _logger.LogInformation("Created a new customer with account.");
                    }
                    catch(Exception e)
                    {
                        _logger.LogInformation("Error a new customer with account.");
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    SendMail.sendMail($"Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấn vào đây</a>.", Input.Email,
                        "Xác nhận email của bạn");
                  
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                    return RedirectToPage("./ConfirmationEmailRegister");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
