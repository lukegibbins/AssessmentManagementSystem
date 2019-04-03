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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DateTime CurrentDate = DateTime.Now;

        // Used as a confirmation message. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        // Variable to store MCQ data once cast from Question
        [BindProperty]
        public MCQ MultipleChoice { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

        // Defines answers choices for drop down
        private const string AnswerChoice1 = "Answer choice 1";
        private const string AnswerChoice2 = "Answer choice 2";
        private const string AnswerChoice3 = "Answer choice 3";
        private const string AnswerChoice4 = "Answer choice 4";

        // Sets selection options for correct answers
        public List<SelectListItem> AnswerOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text=AnswerChoice1, Value=AnswerChoice1},
              new SelectListItem() { Text=AnswerChoice2, Value=AnswerChoice2},
              new SelectListItem() { Text=AnswerChoice3, Value=AnswerChoice3},
              new SelectListItem() { Text=AnswerChoice4, Value=AnswerChoice4},
        };

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // The id here is passed from the question controller
        public async Task<IActionResult> OnGet(int? id)
        {
            // Check if id is null
            if (id == null)
            {
                return NotFound();
            }

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


        // No PostAsync method for viewing details. There is no post request being made

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