using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Assessments
{
    public class CheckYourAnswersModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public string CurrentUserId { get; set; }

        public Assessment Assessment { get; set; }
        public IList<UserAnswer> UserAnswers { get; set; }

        public IList<Question> Questions { get; set; }
        public int QuestionsAnswered { get; set; }

        public int Timer;

        public CheckYourAnswersModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            int hasPageBeenRefreshed = (int)HttpContext.Session.GetInt32("Navigation");
            if (hasPageBeenRefreshed == 3)
            {
                return NotFound();
            }

            Timer = (int)HttpContext.Session.GetInt32("Timer");

            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user.Id;

            if (id == null)
            {
                return NotFound();
            }

            Assessment = await _db.Assessments.FirstOrDefaultAsync(x => x.Id == id);

            if (Assessment == null)
            {
                return NotFound();
            }

            // Select all answers for the logged in user for that specific assessment
            UserAnswers = _db.UserAnswers.Where(c => c.UserId == CurrentUserId && c.AssessmentId == id).ToList();

            // .. and gets all questions for that assessment
            Questions = _db.Questions.Where(q => q.AssessmentId == Assessment.Id && q.Include == true).ToList();

            // Count the number of questions not answered
            QuestionsAnswered = UserAnswers.Where(c => c.AnswerGiven != null).Count();

            HttpContext.Session.SetInt32("Navigation", 0);

            return Page();
        }
    }
}