using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveAssessment.Pages.Questions
{
    [Authorize(Policy = "Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Assessment Assessment;

        [TempData]
        public string Message { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get all assessments from the database
        public IList<Question> GetAllQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if id is null
            if (id == null)
            {
                return NotFound();
            }

            // Get Assessment value to obtain name
            Assessment = await _db.Assessments.FindAsync(id);

            // If no assessment
            if (Assessment == null)
            {
                return NotFound();
            }

            // First need an IQueryable object
            IQueryable<Question> queryQuestions = from allQuestions in _db.Questions select allQuestions;

            // Get all questions where the assessment Id being passed through
            // .. is equal to the assessmentId on the question property.
            GetAllQuestions = queryQuestions.Where(q => q.AssessmentId == id).ToList();

            return Page();
        }
    }
}