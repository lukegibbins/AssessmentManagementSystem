using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Assessments
{
    public class AssessmentDetailsModel : PageModel
    {
        public Assessment Assessment { get; set; }
        public ApplicationUser AssessmentCreator { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public string CurrentUserId { get; set; }

        public AssessmentDetailsModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Set session variables. Initialising values here to act as indexers for current question
            HttpContext.Session.SetInt32("Indexer", 1);
            HttpContext.Session.SetInt32("QuestionIndexer", 0);

            if (id == null)
            {
                return NotFound();
            }

            // Get the assessment being
            Assessment = await _db.Assessments.FindAsync(id);

            try
            {
                // Sets duration for assessment using a session val
                HttpContext.Session.SetInt32("Timer", (Assessment.Duration * 60) - 1);

                // Sets navigation setting
                HttpContext.Session.SetInt32("Navigation", 1);

                // Get the assessment creators name
                AssessmentCreator = await _db.Users.FirstOrDefaultAsync(x => x.Id == Assessment.UserId);

                if (Assessment == null)
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }

            // Removes all user answers from the assessment.
            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user.Id.ToString();

            List<UserAnswer> userAnswers = new List<UserAnswer>();
            userAnswers = await _db.UserAnswers.Where(c => c.UserId == CurrentUserId).ToListAsync();

            try
            {
                _db.RemoveRange(userAnswers);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return Page();
            }

            return Page();
        }
    }
}