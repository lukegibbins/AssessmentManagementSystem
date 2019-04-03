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

namespace InteractiveAssessment.Pages.Stats
{
    [Authorize(Policy = "Teacher")]
    public class AssessmentsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public IList<Assessment> Assessments { get; set; }
        public IList<UserResult> Results { get; set; }
        public List<int> AssessmentPassRate = new List<int>();


        public AssessmentsModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            string currentUserId = user.Id;

            Assessments = await _db.Assessments.Where(c => c.UserId == currentUserId).ToListAsync();

            foreach(var assessment in Assessments)
            {
                int failCount = 0;
                int passCount = 0;

                try
                {
                    Results = await _db.UserResults.Where(c => c.AssessmentId == assessment.Id).ToListAsync();
                    foreach(var res in Results)
                    {
                        if(res.Score >= 40)
                        {
                            passCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    int total = passCount + failCount;
                    decimal passRate = (decimal)passCount / (decimal)total * 100m;
                    int passRateToInt = (int)passRate;
                    AssessmentPassRate.Add(passRateToInt);
                    Results.Clear();
                }
                catch
                {
                    AssessmentPassRate.Add(-1);
                }
            }

            return Page();
        }
    }
}