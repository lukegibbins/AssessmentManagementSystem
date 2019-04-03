using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using InteractiveAssessment.Models.QuestionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages.Questions.MultipleChoice
{
    [Authorize(Policy = "Teacher")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

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

        public EditModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
            MultipleChoice = (MCQ)Question;

            if (MultipleChoice == null || Question == null)
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

            // Extract correct answer string from CorrectAnswer drop down list
            ExtractCorrectAnswerString();

            // Handle on post for image upload here.
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");

                // Get the file extension of file that has been uploaded
                var fileExtension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, MultipleChoice.Id + fileExtension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, MultipleChoice.Id + fileExtension));
                }

                // Create a file using the route of the file, the file extension of the uploaded file
                // and the Id of the question and store in the images folder under the name of the QuestionId
                using (var fileStream = new FileStream(Path.Combine(uploads, MultipleChoice.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MultipleChoice.Image = @"\images\" + MultipleChoice.Id + fileExtension;
            }

            // Find MCQ question 
            MCQ MultipleChoiceFromDb = (MCQ)_db.Questions.Find(MultipleChoice.Id);

            // Check if question has been modified, If so, increment versionNo.
            if (MultipleChoiceFromDb.QuestionTitle != MultipleChoice.QuestionTitle ||
                MultipleChoiceFromDb.Answer1 != MultipleChoice.Answer1 ||
                MultipleChoiceFromDb.Answer2 != MultipleChoice.Answer2 ||
                MultipleChoiceFromDb.Answer3 != MultipleChoice.Answer3 ||
                MultipleChoiceFromDb.Answer4 != MultipleChoice.Answer4 ||
                MultipleChoiceFromDb.CorrectAnswer != MultipleChoice.CorrectAnswer ||
                MultipleChoiceFromDb.Image != MultipleChoice.Image ||
                MultipleChoiceFromDb.CorrectAnswerStringValue != MultipleChoice.CorrectAnswerStringValue)
            {
                MultipleChoiceFromDb.VersionNo = MultipleChoiceFromDb.VersionNo + .1;
            }

            if (MultipleChoice.Image != null)
            {
                MultipleChoiceFromDb.Image = MultipleChoice.Image;
            }
            MultipleChoiceFromDb.QuestionTitle = MultipleChoice.QuestionTitle;
            MultipleChoiceFromDb.Answer1 = MultipleChoice.Answer1;
            MultipleChoiceFromDb.Answer2 = MultipleChoice.Answer2;
            MultipleChoiceFromDb.Answer3 = MultipleChoice.Answer3;
            MultipleChoiceFromDb.Answer4 = MultipleChoice.Answer4;
            MultipleChoiceFromDb.CorrectAnswer = MultipleChoice.CorrectAnswer;
            MultipleChoiceFromDb.CorrectAnswerStringValue = MultipleChoice.CorrectAnswerStringValue;
            MultipleChoiceFromDb.Feedback = MultipleChoice.Feedback;
            MultipleChoiceFromDb.LastUpdated = MultipleChoice.LastUpdated;
            MultipleChoiceFromDb.Include = MultipleChoice.Include;

            await _db.SaveChangesAsync();
            Message = "Question Updated Successfully!";
            return RedirectToPage("/Questions/Index", new { id = MultipleChoice.AssessmentId });
        }

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