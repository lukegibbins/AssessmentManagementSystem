using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using InteractiveAssessment.Models.QuestionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Assessments
{
    public class ResultsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public string CurrentUserId { get; set; }

        public Assessment Assessment { get; set; }
        public IList<UserAnswer> UserAnswers { get; set; }
        public IList<Question> Questions { get; set; }

        // Must be initialised
        public List<Statistic> Statistics = new List<Statistic>();

        public int QuestionCount { get; set; }
        public int QuestionsCorrect { get; set; }
        public int decimalToInt { get; set; }
        public string camelCaseString { get; set; }

        public UserResult UserResult { get; set; }
        public StudentResult StudentResult { get; set; }

        public ResultsModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
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

            // Calculate score
            int indexer = 0;
            int scoreIncrementer = 0;
            foreach (var answer in UserAnswers)
            {
                // This holds the answer given as a string for each answer type
                string answerString = answer.AnswerGiven;
                
                var question = Questions[indexer];
                if(question.QuestionType == "MCQ")
                {
                    MCQ MCQ = (MCQ)question;
                    if(MCQ.CorrectAnswerStringValue == answerString)
                    {
                        // Increment score
                        scoreIncrementer++;

                        // Add question statisics to statistics table if a pass
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, true, question.VersionNo));

                    }
                    else
                    {
                        // and a fail
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, false, question.VersionNo));
                    }
                }
                else if(question.QuestionType == "True/False")
                {
                    TrueFalseOption TrueFalse = (TrueFalseOption)question;
                    if (TrueFalse.CorrectAnswer == answerString)
                    {
                        // Increment score
                        scoreIncrementer++;

                        // Add question statisics to statistics table if a pass
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, true, question.VersionNo));

                    }
                    else
                    {
                        // and a fail
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, false, question.VersionNo));
                    }
                }
                else if (question.QuestionType == "Short Answer")
                {
                    ShortAnswerOption ShortAnswer = (ShortAnswerOption)question;
                    camelCaseString = UpperCaseFirst(answerString);

                    // If the correct answer to the short answer question is equal to the given answer which is either
                    // A) standar formt, B) camel case, C upper case)

                    if (answerString != null)
                    {
                        if (ShortAnswer.CorrectAnswer == answerString ||
                            ShortAnswer.CorrectAnswer == camelCaseString ||
                            ShortAnswer.CorrectAnswer == answerString.ToUpper())
                        {
                            // Increment score
                            scoreIncrementer++;

                            // Add question statisics to statistics table if a pass
                            Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, true, question.VersionNo));

                        }
                    }
                    else
                    {
                        // and a fail
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, false, question.VersionNo));
                    }
                }
                else if (question.QuestionType == "Interactive")
                {
                    Interactive Interactive = (Interactive)question;
                  
                    // If the rows representing the names that the new re-ordered match the row values specified
                    // as the correct values, then the answer is correct
                    if (answerString == Interactive.CorrectNameListing)
                    {
                        // Increment score
                        scoreIncrementer++;

                        // Add question statisics to statistics table if a pass
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, true, question.VersionNo));

                    }
                    else
                    {
                        // and a fail
                        Statistics.Add(new Statistic(question.Id, answer.UserId, answer.AssessmentId, false, question.VersionNo));
                    }
                }

                // Increment the indexer
                indexer++;
            }

            // Re-format score to string
            QuestionsCorrect = scoreIncrementer;
            decimal decimalScore = (decimal) QuestionsCorrect / (decimal)Questions.Count() * 100m;
            decimalToInt = (int)decimalScore;

            // Save score to UserResult table in DB
            // As this isn't an object that is being bound from a form, we have to manually initialise it using 
            // .. the new keyword.
            UserResult = new UserResult();
            UserResult.UserId = CurrentUserId;
            UserResult.AssessmentName = Assessment.AssessmentName;
            UserResult.Score = decimalToInt;
            UserResult.DateTaken = DateTime.Now;
            UserResult.AssessmentId = (int)id;

            // Duplicate the result to store in a separate student results table to allow teachers to safely delete
            // .. student assessment results.
            StudentResult = new StudentResult();
            StudentResult.StudentUserId = UserResult.UserId;
            StudentResult.AssessmentName = UserResult.AssessmentName;
            StudentResult.StudentScore = UserResult.Score;
            StudentResult.DateTaken = UserResult.DateTaken;
            StudentResult.AssessmentId = UserResult.AssessmentId;

            // Add User Result to db
            _db.UserResults.Add(UserResult);
            _db.StudentResults.Add(StudentResult);

            // Add these list of statistics to the statistics table
            foreach(var stat in Statistics)
            {
                _db.Statistics.Add(stat);
            }

            // Save all changes.
            await _db.SaveChangesAsync();

            // Make navigatiom backward unaccessible
            HttpContext.Session.SetInt32("Navigation", 3);

            return Page();
        }

        public async Task<IActionResult> OnGetFinish()
        {
            // Removes all user answers from the assessment when the user clicks the finish button.
            // .. alternatively, the user can clear all user answers by switching the assessment active status to
            // .. 'inactive'.
            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user.Id.ToString();

            List<UserAnswer> userAnswers = new List<UserAnswer>();
            userAnswers = await _db.UserAnswers.Where(c => c.UserId == CurrentUserId).ToListAsync();
            _db.RemoveRange(userAnswers);

            await _db.SaveChangesAsync();
            return RedirectToPage("/Index");
        }

        private string UpperCaseFirst(string answer)
        {
            if (string.IsNullOrEmpty(answer))
            {
                return string.Empty;
            }
            char[] a = answer.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}

