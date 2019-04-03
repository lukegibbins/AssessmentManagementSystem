using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using InteractiveAssessment.Models.QuestionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Data.Entity.Migrations;
using System.Diagnostics;

namespace InteractiveAssessment.Pages.Assessments
{
    public class StartModel : PageModel
    {
        public Assessment Assessment { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;

        public IList<Question> Questions { get; set; }
        public IList<Interactive> Interactives { get; set; }
        public Question CurrentQuestion { get; set; }

        public string CurrentUserId { get; set; }
        private readonly ApplicationDbContext _db;
        public int UserAnswerPosition { get; set; }

        [BindProperty]
        public UserAnswer UserAnswer { get; set; }

        // Defines what question and element question needs to be output
        public int Indexer;
        public int QuestionIndexer;
        public int Timer;

        public StartModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        // Get assessment Id for the assessment
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                //Gets navigation value
                int hasPageBeenRefreshed = (int)HttpContext.Session.GetInt32("Navigation");

                // This method determines whether the page has been refreshed or not. If nav == 0,
                // .. we know the page has been refreshed, if it is '1', then it has been redirected.
                if (hasPageBeenRefreshed == 0)
                {
                    DateTime currentDateTime = DateTime.Now;
                    var startOfQuestion = (string)HttpContext.Session.GetString("StartingQuestion");
                    DateTime startOfQuestionDateFormat = DateTime.Parse(startOfQuestion);

                    int secondsDiff = (int)currentDateTime.Subtract(startOfQuestionDateFormat).TotalSeconds;
                    Timer = (int)HttpContext.Session.GetInt32("Timer");
                    int diffBetweenOriginalTimer = Timer - secondsDiff;
                    HttpContext.Session.SetInt32("Timer", diffBetweenOriginalTimer);

                }
                // Prevent user from going backward from the end result.
                else if (hasPageBeenRefreshed == 3)
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }

            // Gets the session values from the Assessment Brief page where the values are initialised
            Indexer = (int)HttpContext.Session.GetInt32("Indexer");
            QuestionIndexer = (int)HttpContext.Session.GetInt32("QuestionIndexer");

            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user.Id;

            if (id == null)
            {
                return NotFound();
            }

            Assessment = await _db.Assessments.FirstOrDefaultAsync(x => x.Id == id);

            // If no assessment
            if (Assessment == null)
            {
                return NotFound();
            }

            Questions = _db.Questions.Where(q => q.AssessmentId == Assessment.Id && q.Include == true).ToList();

            if (Questions == null)
            {
                return NotFound();
            }

            // Get out first question regardless of it's type and display on the view
            CurrentQuestion = Questions.ElementAtOrDefault(QuestionIndexer);

            // If the question has already been answered, find the previously given answer and the string value
            try
            {
                UserAnswer hasAnswerBeenGiven = _db.UserAnswers.SingleOrDefault(
                    c => c.QuestionId == CurrentQuestion.Id && c.UserId == CurrentUserId);

                // Assign val to update. This will have a value if the question has already been answered
                UserAnswer = hasAnswerBeenGiven;

                string hasStringVarBeenProvided = hasAnswerBeenGiven.AnswerGiven;
                if (hasStringVarBeenProvided != null)
                {
                    UserAnswerPosition = hasAnswerBeenGiven.AnswerPosition;
                }
            }
            catch
            {
                UserAnswerPosition = 0;
            }

            // Timer details
            Timer = (int)HttpContext.Session.GetInt32("Timer");
            HttpContext.Session.SetString("StartingQuestion", DateTime.Now.ToString());

            HttpContext.Session.SetInt32("Navigation", 0);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int[] rowId, string[] names)
        {
            // Calculate time taken between user answering question and moving to next question
            DateTime currentDateTime = DateTime.Now;
            var startOfQuestion = (string)HttpContext.Session.GetString("StartingQuestion");
            DateTime startOfQuestionDateFormat = DateTime.Parse(startOfQuestion);

            int secondsDiff = (int)currentDateTime.Subtract(startOfQuestionDateFormat).TotalSeconds;
            Timer = (int)HttpContext.Session.GetInt32("Timer");
            int diffBetweenOriginalTimer = Timer - secondsDiff;
            HttpContext.Session.SetInt32("Timer", diffBetweenOriginalTimer);

            Indexer = (int)HttpContext.Session.GetInt32("Indexer");
            QuestionIndexer = (int)HttpContext.Session.GetInt32("QuestionIndexer");

            Assessment = await _db.Assessments.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user.Id;

            Questions = _db.Questions.Where(q => q.AssessmentId == Assessment.Id && q.Include == true).ToList();

            if (Questions == null)
            {
                return NotFound();
            }

            // At this point, CurrentQuestion has No Value until it's recreated
            // .. so lets recreate it. We get out the same CurrentQuestion as the OnGet()
            // .. as the index has not changed; the indexer is static
            CurrentQuestion = Questions.ElementAtOrDefault(QuestionIndexer);

            // If the question type of the current question is of type MCQ
            // .. then extract out the Users string answers and Answer position.
            if (CurrentQuestion.QuestionType == "MCQ")
            {
                MCQ MCQ = (MCQ)CurrentQuestion;
                IDictionary<int, string> AnswerDictionary = new Dictionary<int, string>();
                AnswerDictionary.Add(new KeyValuePair<int, string>(0, null));
                AnswerDictionary.Add(new KeyValuePair<int, string>(1, MCQ.Answer1));
                AnswerDictionary.Add(new KeyValuePair<int, string>(2, MCQ.Answer2));
                AnswerDictionary.Add(new KeyValuePair<int, string>(3, MCQ.Answer3));
                AnswerDictionary.Add(new KeyValuePair<int, string>(4, MCQ.Answer4));

                UserAnswer.AnswerGiven = AnswerDictionary[UserAnswer.AnswerPosition];
            }

            else if (CurrentQuestion.QuestionType == "True/False")
            {
                TrueFalseOption TrueFalse = (TrueFalseOption)CurrentQuestion;

                IDictionary<int, string> AnswerDictionary = new Dictionary<int, string>();
                AnswerDictionary.Add(new KeyValuePair<int, string>(0, null));
                AnswerDictionary.Add(new KeyValuePair<int, string>(1, "True"));
                AnswerDictionary.Add(new KeyValuePair<int, string>(2, "False"));

                UserAnswer.AnswerGiven = AnswerDictionary[UserAnswer.AnswerPosition];
            }

            else if (CurrentQuestion.QuestionType == "Interactive")
            {
                UserAnswer.hasInteractiveBeenAnswered = true;
                UserAnswer.InteractiveName1 = names[0];
                UserAnswer.InteractiveName2 = names[1];
                UserAnswer.InteractiveName3 = names[2];
                UserAnswer.InteractiveName4 = names[3];
                UserAnswer.RowNo1 = rowId[0];
                UserAnswer.RowNo2 = rowId[1];
                UserAnswer.RowNo3 = rowId[2];
                UserAnswer.RowNo4 = rowId[3];
                UserAnswer.AnswerGiven = UserAnswer.InteractiveName1
                    + ", " + UserAnswer.InteractiveName2
                    + ", " + UserAnswer.InteractiveName3
                    + ", " + UserAnswer.InteractiveName4;
            }

            // try update. If the UserAnswer doesn't exist or can't be updated.. then create one.
            try
            {
                // Update an answer
                UserAnswer newUserAnswer = _db.UserAnswers.SingleOrDefault(
                    c => c.QuestionId == CurrentQuestion.Id && c.UserId == CurrentUserId);
                newUserAnswer.AnswerPosition = UserAnswer.AnswerPosition;
                newUserAnswer.AnswerGiven = UserAnswer.AnswerGiven;

                if (newUserAnswer.hasInteractiveBeenAnswered == true)
                {
                    newUserAnswer.InteractiveName1 = names[0];
                    newUserAnswer.InteractiveName2 = names[1];
                    newUserAnswer.InteractiveName3 = names[2];
                    newUserAnswer.InteractiveName4 = names[3];
                    newUserAnswer.RowNo1 = rowId[0];
                    newUserAnswer.RowNo2 = rowId[1];
                    newUserAnswer.RowNo3 = rowId[2];
                    newUserAnswer.RowNo4 = rowId[3];
                    newUserAnswer.AnswerGiven = newUserAnswer.InteractiveName1
                   + ", " + newUserAnswer.InteractiveName2
                   + ", " + newUserAnswer.InteractiveName3
                   + ", " + newUserAnswer.InteractiveName4;
                }

                _db.Attach(newUserAnswer).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch
            {
                // Create an answer
                _db.UserAnswers.Add(UserAnswer);
                await _db.SaveChangesAsync();
            }

            // Increase the incrementer
            Indexer++;
            QuestionIndexer++;
            HttpContext.Session.SetInt32("Indexer", Indexer);
            HttpContext.Session.SetInt32("QuestionIndexer", QuestionIndexer);

            if (Indexer > Questions.Count)
            {
                Indexer--;
                QuestionIndexer--;
                HttpContext.Session.SetInt32("Indexer", Indexer);
                HttpContext.Session.SetInt32("QuestionIndexer", QuestionIndexer);
                HttpContext.Session.SetString("StartingQuestion", DateTime.Now.ToString());
                return RedirectToPage("/Assessments/CheckYourAnswers", new { id = Assessment.Id });
            }
            else
            {
                // Redirect back to the OnGet. The static vals should remain the same
                HttpContext.Session.SetInt32("Navigation", 1);
                return RedirectToPage("/Assessments/Start", new { id = Assessment.Id });
            }
        }

        public IActionResult OnGetPrevious(int? Id)
        {
            // Calculate time taken between user answering question and moving to next question
            DateTime currentDateTime = DateTime.Now;
            var startOfQuestion = (string)HttpContext.Session.GetString("StartingQuestion");
            DateTime startOfQuestionDateFormat = DateTime.Parse(startOfQuestion);

            int secondsDiff = (int)currentDateTime.Subtract(startOfQuestionDateFormat).TotalSeconds;
            Timer = (int)HttpContext.Session.GetInt32("Timer");
            int diffBetweenOriginalTimer = Timer - secondsDiff;
            HttpContext.Session.SetInt32("Timer", diffBetweenOriginalTimer);

            // Go to previous question by decrementing counters and redirecting back to OnGet()
            Indexer = (int)HttpContext.Session.GetInt32("Indexer");
            QuestionIndexer = (int)HttpContext.Session.GetInt32("QuestionIndexer");

            Indexer--;
            QuestionIndexer--;
            HttpContext.Session.SetInt32("Indexer", Indexer);
            HttpContext.Session.SetInt32("QuestionIndexer", QuestionIndexer);
            HttpContext.Session.SetInt32("Navigation", 1);

            return RedirectToPage("/Assessments/Start", new { id = Id });
        }
    }
}





































