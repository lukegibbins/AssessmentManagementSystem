using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveAssessment.Pages.Questions.ShortAnswer
{
    [Authorize(Policy = "Teacher")]
    public class DeleteModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Binds data sent from the create assessment request
        [BindProperty]
        public ShortAnswerOption ShortAnswer { get; set; }

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

        // Returns the create page to the end user
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Get the Question relating to the one being edited using the id
            Question = await _db.Questions.FindAsync(id);

            // Cast the question as its intended class
            ShortAnswer = (ShortAnswerOption)Question;

            if (ShortAnswer == null || Question == null)
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
            ShortAnswerOption DeletingShortAnswer = (ShortAnswerOption)Question;

            if (DeletingShortAnswer != null)
            {
                // Remove assessment from database
                _db.Questions.Remove(DeletingShortAnswer);
                await _db.SaveChangesAsync();
                Message = "Short Answer Deleted Successfully!";
            }

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("/Questions/Index", new { id = DeletingShortAnswer.AssessmentId });
        }
    }
}