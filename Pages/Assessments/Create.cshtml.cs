using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveAssessment.Pages.Assessments
{
    [Authorize(Policy = "Teacher")]
    public class CreateModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Used to get current user
        private readonly UserManager<ApplicationUser> _userManager;

        // Binds data sent from the create assessment request
        [BindProperty]
        public Assessment Assessment { get; set; }

        // Get current date and time
        public DateTime CurrentDate = DateTime.Now;

        // Store CurrentUserId
        public string CurrentUserId;

        // Temp data which confirms database transation. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorQuestionMessage { get; set; }

        // Random string generator
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Stores randomly generated string
        public string AssessmentPassword = RandomString(8);

        // Constructor
        public CreateModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        // Returns the create page to the end user
        public async Task<IActionResult> OnGetAsync()
        {
            // Find current users ID
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Now holds the user id to store as a foreign key in the assessments table for user
            CurrentUserId = user.Id;

            return Page();
        }

        // When a request is sent ON the OnGet() or create page
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Adds the bound Assessment to the assessment table in the database
            _db.Assessments.Add(Assessment);
            await _db.SaveChangesAsync();
            Message = "Assessment Created Successfully!";

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("./Index");
        }
    }
}