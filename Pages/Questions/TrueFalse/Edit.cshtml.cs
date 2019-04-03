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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InteractiveAssessment.Pages.Questions.TrueFalse
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
        public TrueFalseOption TrueFalse { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

        // Defines answers choices for drop down
        private const string TrueAnswer = "True";
        private const string FalseAnswer = "False";

        // Sets selection options for correct answers
        public List<SelectListItem> AnswerOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text=TrueAnswer, Value=TrueAnswer},
              new SelectListItem() { Text=FalseAnswer, Value=FalseAnswer}
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
            TrueFalse = (TrueFalseOption)Question;

            if (TrueFalse == null || Question == null)
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

                if (System.IO.File.Exists(Path.Combine(uploads, TrueFalse.Id + fileExtension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, TrueFalse.Id + fileExtension));
                }

                // Create a file using the route of the file, the file extension of the uploaded file
                // and the Id of the question and store in the images folder under the name of the QuestionId
                using (var fileStream = new FileStream(Path.Combine(uploads, TrueFalse.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                TrueFalse.Image = @"\images\" + TrueFalse.Id + fileExtension;
            }

            // Find MCQ question 
            TrueFalseOption TrueFalseFromDb = (TrueFalseOption)_db.Questions.Find(TrueFalse.Id);

            // Check if question has been modified, If so, increment versionNo.
            if (TrueFalseFromDb.QuestionTitle != TrueFalse.QuestionTitle ||
                TrueFalseFromDb.CorrectAnswer != TrueFalse.CorrectAnswer ||
                TrueFalseFromDb.Image != TrueFalse.Image)
            {
                TrueFalseFromDb.VersionNo = TrueFalseFromDb.VersionNo + .1;
            }

            if (TrueFalse.Image != null)
            {
                TrueFalseFromDb.Image = TrueFalse.Image;
            }
            TrueFalseFromDb.QuestionTitle = TrueFalse.QuestionTitle;
            TrueFalseFromDb.CorrectAnswer = TrueFalse.CorrectAnswer;
            TrueFalseFromDb.Feedback = TrueFalse.Feedback;
            TrueFalseFromDb.LastUpdated = TrueFalse.LastUpdated;
            TrueFalseFromDb.Include = TrueFalse.Include;

            await _db.SaveChangesAsync();
            Message = "Question Updated Successfully!";
            return RedirectToPage("/Questions/Index", new { id = TrueFalse.AssessmentId });
        }
    }
}