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

namespace InteractiveAssessment.Pages.Questions.DragnDrop
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
        public Interactive Interactive { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

        public SelectList AnswerOptions { get; set; }
        List<int> IntValues = new List<int> { 1, 2, 3, 4 };

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
            AnswerOptions = new SelectList(IntValues);
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
            Interactive = (Interactive)Question;

            if (Interactive == null || Question == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}