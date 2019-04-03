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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string Message { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

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

        [BindProperty]
        public Assessment Assessment { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            // Get the assessment that has been selected from the index page using the Id passed through 
            Assessment = await _db.Assessments.SingleOrDefaultAsync(c => c.Id == id);

            if (Assessment == null)
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

            // Alternative to SingleOrDefault
            Assessment = await _db.Assessments.FindAsync(id);

            if (Assessment != null)
            {
                // Remove assessment from database
                _db.Assessments.Remove(Assessment);
                await _db.SaveChangesAsync();
                Message = "Assessment Deleted Successfully!";
            }

            return RedirectToPage("./Index");
        }
    }
}