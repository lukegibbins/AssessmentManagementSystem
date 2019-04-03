using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using InteractiveAssessment.Data;
using InteractiveAssessment.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InteractiveAssessment.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;

        // New instances to set roles for different user types
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        // String const variables to define account type (student or teacher)
        public const string StudentUser = "Student";
        public const string TeacherUser = "Teacher";

        // Select list for user to select from
        public List<SelectListItem> AccountTypeStatus = new List<SelectListItem>()
        {
              new SelectListItem() { Text="Student Account", Value=StudentUser},
              new SelectListItem() { Text="Teacher Account", Value=TeacherUser}
        };

        // Updated ctor and values with instances of RoleManager and ApplicationDbContext
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
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
            [Display(Name = "First name")]
            public string FirstName { get; set; } //New field

            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; } //New field

            [Required]
            [Display(Name = "Account type")]
            public string AccountType { get; set; } //New field

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                // Create new instance of ApplicationUser with the details on the register form
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    Surname = Input.Surname,
                    AccountType = Input.AccountType
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Check if theres a role in the Db called Student and create if not
                    if(!await _roleManager.RoleExistsAsync(StudentUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StudentUser));
                    }

                    // Check if theres a role in the Db called Teacher and create if not
                    if (!await _roleManager.RoleExistsAsync(TeacherUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(TeacherUser));
                    }

                    if(user.AccountType == StudentUser)
                    {
                        // Add the user as a student if student account type is selected
                        await _userManager.AddToRoleAsync(user, StudentUser);
                    }
                    else if (user.AccountType == TeacherUser)
                    {
                        // Add the user as a teacher if teacher account type is selected
                        await _userManager.AddToRoleAsync(user, TeacherUser);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(Input.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
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
