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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DateTime CurrentDate = DateTime.Now;

        // Variable to store True false data once cast from Question
        [BindProperty]
        public ShortAnswerOption ShortAnswer { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

        // Defines answers choices for drop down
        private const string TrueAnswer = "True";
        private const string FalseAnswer = "False";

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
            ShortAnswer = (ShortAnswerOption)Question;

            if (ShortAnswer == null || Question == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}