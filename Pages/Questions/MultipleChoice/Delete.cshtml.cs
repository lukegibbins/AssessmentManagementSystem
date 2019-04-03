using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using InteractiveAssessment.Models.QuestionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InteractiveAssessment.Pages.Questions.MultipleChoice
{
    [Authorize(Policy = "Teacher")]
    public class DeleteModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Defines answers choices for drop down
        private const string AnswerChoice1 = "Answer choice 1";
        private const string AnswerChoice2 = "Answer choice 2";
        private const string AnswerChoice3 = "Answer choice 3";
        private const string AnswerChoice4 = "Answer choice 4";

        // Binds data sent from the create assessment request
        [BindProperty]
        public MCQ MultipleChoice { get; set; }

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
              new SelectListItem() { Text=AnswerChoice1, Value=AnswerChoice1},
              new SelectListItem() { Text=AnswerChoice2, Value=AnswerChoice2},
              new SelectListItem() { Text=AnswerChoice3, Value=AnswerChoice3},
              new SelectListItem() { Text=AnswerChoice4, Value=AnswerChoice4},
        };

        // Returns the create page to the end user
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Get the Question relating to the one being edited using the id
            Question = await _db.Questions.FindAsync(id);

            // Cast the question as its intended class
            MultipleChoice = (MCQ) Question;

            if (MultipleChoice == null || Question == null)
            {
                return NotFound();
            }

            return Page();
        }

        // When a request is sent ON the OnGet() or create page
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            // Asnchronyously find the item to be deleted and conver it to MCQ to delete
            Question = await _db.Questions.FindAsync(id);
            MCQ DeletingMultipleChoice = (MCQ) Question;

            if (DeletingMultipleChoice != null)
            {
                // Remove assessment from database
                _db.Questions.Remove(DeletingMultipleChoice);
                await _db.SaveChangesAsync();
                Message = "Multiple Choice Deleted Successfully!";
            }

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("/Questions/Index", new { id = DeletingMultipleChoice.AssessmentId });
        }


        // Find the correct answer string value using the selecting from the drop down list
        private void ExtractCorrectAnswerString()
        {
            if (MultipleChoice.CorrectAnswer == AnswerChoice1)
            {
                MultipleChoice.CorrectAnswerStringValue = MultipleChoice.Answer1.ToString();
            }
            else if (MultipleChoice.CorrectAnswer == AnswerChoice2)
            {
                MultipleChoice.CorrectAnswerStringValue = MultipleChoice.Answer2.ToString();
            }
            else if (MultipleChoice.CorrectAnswer == AnswerChoice3)
            {
                MultipleChoice.CorrectAnswerStringValue = MultipleChoice.Answer3.ToString();
            }
            else if (MultipleChoice.CorrectAnswer == AnswerChoice4)
            {
                MultipleChoice.CorrectAnswerStringValue = MultipleChoice.Answer4.ToString();
            }
        }
    }
}