using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Stats
{
    [Authorize(Policy = "Teacher")]
    public class StudentModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IList<UserResult> UserResults { get; set; }
        public List<Student> StudentNames = new List<Student>();
        public Assessment Assessment { get; set; }

        [TempData]
        public string Message { get; set; }

        public StudentModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Assessment = _db.Assessments.Find(id);
            UserResults = await _db.UserResults.Where(c => c.AssessmentId == id).ToListAsync();

            foreach(var result in UserResults)
            {
                ApplicationUser User = await _db.Users.FirstOrDefaultAsync(x => x.Id == result.UserId);
                StudentNames.Add(new Student(User.Email, User.FullName));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int? id)
        {
            var userResult = await _db.UserResults.FindAsync(id);

            _db.UserResults.Remove(userResult);
            await _db.SaveChangesAsync();
            Message = "Student result deleted successfully!";
            return RedirectToPage(new { id = userResult.AssessmentId });
        }
    }

    public class Student
    {
        public string Email { get; set; }
        public string FullName { get; set; }

        public Student(string email, string fullName)
        {
            FullName = fullName;
            Email = email;
        }
    }
}
