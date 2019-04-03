using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Results
{
    public class StudentModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IList<StudentResult> StudentResults { get; set; }

        [TempData]
        public string Message { get; set; }

        public StudentModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            string currentUserId = user.Id;

            StudentResults = await _db.StudentResults.Where(c => c.StudentUserId == currentUserId).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int? id)
        {
            var studentResult = await _db.StudentResults.FindAsync(id);

            _db.StudentResults.Remove(studentResult);
            await _db.SaveChangesAsync();
            Message = "Your result has been deleted successfully!";
            return RedirectToPage();
        }
    }
}