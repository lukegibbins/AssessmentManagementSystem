using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InteractiveAssessment.Pages.Questions.TrueFalse
{
    [Authorize(Policy = "Teacher")]
    public class DeleteModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Defines answers choices for drop down
        private const string TrueAnswer = "True";
        private const string FalseAnswer = "False";

        // Binds data sent from the create assessment request
        [BindProperty]
        public TrueFalseOption TrueFalse { get; set; }

        public Question Question { get; set; }

        // Temp data which confirms database transation. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        // Get current date and time
        public DateTime CurrentDate = DateTime.Now;

        // Constructor
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // Sets selection options for correct answers
        public List<SelectListItem> AnswerOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text=TrueAnswer, Value=TrueAnswer},
              new SelectListItem() { Text=FalseAnswer, Value=FalseAnswer},
        };

        // Returns the create page to the end user
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Get the Question relating to the one being edited using the id
            Question = await _db.Questions.FindAsync(id);

            // Cast the question as its intended class
            TrueFalse = (TrueFalseOption)Question;

            if (TrueFalse == null || Question == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Asnchronyously find the item to be deleted and conver it to MCQ to delete
            Question = await _db.Questions.FindAsync(id);
            TrueFalseOption DeletingTrueFalse = (TrueFalseOption)Question;

            if (DeletingTrueFalse != null)
            {
                // Remove assessment from database
                _db.Questions.Remove(DeletingTrueFalse);
                await _db.SaveChangesAsync();
                Message = "True/False Deleted Successfully!";
            }

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("/Questions/Index", new { id = DeletingTrueFalse.AssessmentId });
        }
    }
}