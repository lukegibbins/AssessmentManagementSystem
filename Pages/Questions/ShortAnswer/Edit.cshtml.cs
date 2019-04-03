using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveAssessment.Pages.Questions.ShortAnswer
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
        public ShortAnswerOption ShortAnswer { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

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
            ShortAnswer = (ShortAnswerOption)Question;

            if (ShortAnswer == null || Question == null)
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

            // Handle on post for image upload here.
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");

                // Get the file extension of file that has been uploaded
                var fileExtension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, ShortAnswer.Id + fileExtension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, ShortAnswer.Id + fileExtension));
                }

                // Create a file using the route of the file, the file extension of the uploaded file
                // and the Id of the question and store in the images folder under the name of the QuestionId
                using (var fileStream = new FileStream(Path.Combine(uploads, ShortAnswer.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                ShortAnswer.Image = @"\images\" + ShortAnswer.Id + fileExtension;
            }

            // Find MCQ question 
            ShortAnswerOption ShortAnswerFromDb = (ShortAnswerOption)_db.Questions.Find(ShortAnswer.Id);

            // Check if question has been modified, If so, increment versionNo.
            if (ShortAnswerFromDb.QuestionTitle != ShortAnswer.QuestionTitle ||
                ShortAnswerFromDb.CorrectAnswer != ShortAnswer.CorrectAnswer ||
                ShortAnswerFromDb.Image != ShortAnswer.Image)
            {
                ShortAnswerFromDb.VersionNo = ShortAnswerFromDb.VersionNo + .1;
            }

            if (ShortAnswer.Image != null)
            {
                ShortAnswerFromDb.Image = ShortAnswer.Image;
            }
            ShortAnswerFromDb.QuestionTitle = ShortAnswer.QuestionTitle;
            ShortAnswerFromDb.CorrectAnswer = ShortAnswer.CorrectAnswer;
            ShortAnswerFromDb.Feedback = ShortAnswer.Feedback;
            ShortAnswerFromDb.LastUpdated = ShortAnswer.LastUpdated;
            ShortAnswerFromDb.Include = ShortAnswer.Include;

            await _db.SaveChangesAsync();
            Message = "Question Updated Successfully!";
            return RedirectToPage("/Questions/Index", new { id = ShortAnswer.AssessmentId });
        }
    }
}