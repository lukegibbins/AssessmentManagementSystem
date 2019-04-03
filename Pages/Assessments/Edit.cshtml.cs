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
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Assessments
{
    [Authorize(Policy = "Teacher")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DateTime CurrentDate = DateTime.Now;

        // Used as a confirmation message. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorQuestionMessage { get; set; }

        // Defines active status options for assessment
        public List<SelectListItem> ActiveOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text="Active", Value="Active"},
              new SelectListItem() { Text="Inactive", Value="Inactive"}
        };

        // Defines duration options for assessment
        public List<SelectListItem> DurationOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text="2", Value="2"},
              new SelectListItem() { Text="4", Value="4"},
              new SelectListItem() { Text="6", Value="6"},
              new SelectListItem() { Text="8", Value="8"},
              new SelectListItem() { Text="10", Value="10"}
        };


        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Assessment Assessment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            // Check if id is null
            if (id == null)
            {
                return NotFound();
            }

            // Get the assessment relating to the assessment where the Id is being passed through
            Assessment = await _db.Assessments.SingleOrDefaultAsync(c => c.Id == id);

            if (Assessment == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update the assessment
            _db.Attach(Assessment).State = EntityState.Modified;      

            // Get question count for this assessment
            int assessmentQuestionCount = _db.Questions.Where(c => c.AssessmentId == Assessment.Id).Count();

            // If the question count is 0 and the assessment has been activated, throw an error
            if(assessmentQuestionCount == 0 && Assessment.ActiveStatus == "Active")
            {
                ErrorQuestionMessage = "An assessment cannot be activated with 0 questions.";
                return Page();
            }

            await _db.SaveChangesAsync();
            Message = "Assessment Updated Successfully!";
            return RedirectToPage("./Index");
        }
    }
}
