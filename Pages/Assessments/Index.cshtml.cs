using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Assessments
{
    [Authorize(Policy = "Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        [TempData]
        public string Message { get; set; }

        public IndexModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        // Get all assessments from the database
        public IList<Assessment> GetAllAssessments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Find current users ID
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get all assessments for the current user and order by last updated.
            GetAllAssessments = await _db.Assessments
                .Where(a => a.UserId == user.Id)
                .OrderBy(a => a.LastUpdated.Date)
                .ToListAsync();

            return Page();
        }
    }
}