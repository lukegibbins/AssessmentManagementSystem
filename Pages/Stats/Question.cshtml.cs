using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Stats
{
    [Authorize(Policy = "Teacher")]
    public class QuestionModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IList<Question> Questions { get; set; }
        public Assessment Assessment { get; set; }

        public List<int> QuestionPassRate = new List<int>();
        public List<Statistic> Statistics = new List<Statistic>();

        public QuestionModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Assessment = _db.Assessments.Find(id);
            Questions = await _db.Questions.Where(c => c.AssessmentId == id).ToListAsync(); 

            // For each question in the assessment
            foreach (var q in Questions)
            {
                int failCount = 0;
                int passCount = 0;

                try
                {  
                    // Get all statistics for that question
                    Statistics = await _db.Statistics.Where(c => c.QuestionId == q.Id && c.VersionNo == q.VersionNo).ToListAsync();

                    // For each of the found statistics
                    foreach (var stat in Statistics)
                    {
                        if (stat.Pass == true)
                        {
                            passCount++;
                        }
                        else if (stat.Pass == false)
                        {
                            failCount++;
                        }
                    }

                    int total = passCount + failCount;
                    decimal passRate = (decimal)passCount / (decimal)total * 100m;
                    int passRateToInt = (int)passRate;
                    QuestionPassRate.Add(passRateToInt);
                    Statistics.Clear();
                }
                // If no statistics are found, add -1 to the list
                catch
                {
                    QuestionPassRate.Add(-1);
                }
            }
            return Page();
        }
    }
}