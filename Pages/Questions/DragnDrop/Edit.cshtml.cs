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

namespace InteractiveAssessment.Pages.Questions.DragnDrop
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
        public Interactive Interactive { get; set; }

        // Variable to store question data for the OnGet
        public Question Question { get; set; }

        public SelectList AnswerOptions { get; set; }
        List<int> IntValues = new List<int> { 1, 2, 3, 4 };

        public EditModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _db = db;
            AnswerOptions = new SelectList(IntValues);
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
            Interactive = (Interactive)Question;

            if (Interactive == null || Question == null)
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

            string[] StringBuilder = new string[4];
            StringBuilder[Interactive.CorrectRowNo1 - 1] = Interactive.Name1;
            StringBuilder[Interactive.CorrectRowNo2 - 1] = Interactive.Name2;
            StringBuilder[Interactive.CorrectRowNo3 - 1] = Interactive.Name3;
            StringBuilder[Interactive.CorrectRowNo4 - 1] = Interactive.Name4;

            Interactive.CorrectNameListing = StringBuilder[0]
                + ", " + StringBuilder[1] + ", " + StringBuilder[2] + ", " + StringBuilder[3];

            // Handle on post for image upload here.
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");

                // Get the file extension of file that has been uploaded
                var fileExtension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, Interactive.Id + fileExtension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, Interactive.Id + fileExtension));
                }

                // Create a file using the route of the file, the file extension of the uploaded file
                // and the Id of the question and store in the images folder under the name of the QuestionId
                using (var fileStream = new FileStream(Path.Combine(uploads, Interactive.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                Interactive.Image = @"\images\" + Interactive.Id + fileExtension;
            }

            // Find MCQ question 
            Interactive InteractiveFromDb = (Interactive)_db.Questions.Find(Interactive.Id);

            // Check if question has been modified, If so, increment versionNo.
            if (InteractiveFromDb.QuestionTitle != Interactive.QuestionTitle ||
                InteractiveFromDb.RowNo1 != Interactive.RowNo1 ||
                InteractiveFromDb.RowNo2 != Interactive.RowNo2 ||
                InteractiveFromDb.RowNo3 != Interactive.RowNo3 ||
                InteractiveFromDb.RowNo4 != Interactive.RowNo4 ||
                InteractiveFromDb.CorrectRowNo1 != Interactive.CorrectRowNo1 ||
                InteractiveFromDb.CorrectRowNo2 != Interactive.CorrectRowNo2 ||
                InteractiveFromDb.CorrectRowNo3 != Interactive.CorrectRowNo3 ||
                InteractiveFromDb.CorrectRowNo4 != Interactive.CorrectRowNo4 ||
                InteractiveFromDb.Image != Interactive.Image ||
                InteractiveFromDb.Name1 != Interactive.Name1 ||
                InteractiveFromDb.Name2 != Interactive.Name2 ||
                InteractiveFromDb.Name3 != Interactive.Name3 ||
                InteractiveFromDb.Name4 != Interactive.Name4)
            {
                InteractiveFromDb.VersionNo = InteractiveFromDb.VersionNo + .1;
            }

            if(Validator() == true && EntryNameValidator() == true)
            {
                if (Interactive.Image != null)
                {
                    InteractiveFromDb.Image = Interactive.Image;
                }
                InteractiveFromDb.QuestionTitle = Interactive.QuestionTitle;
                InteractiveFromDb.RowNo1 = Interactive.RowNo1;
                InteractiveFromDb.RowNo2 = Interactive.RowNo2;
                InteractiveFromDb.RowNo3 = Interactive.RowNo3;
                InteractiveFromDb.RowNo4 = Interactive.RowNo4;
                InteractiveFromDb.CorrectRowNo1 = Interactive.CorrectRowNo1;
                InteractiveFromDb.CorrectRowNo2 = Interactive.CorrectRowNo2;
                InteractiveFromDb.CorrectRowNo3 = Interactive.CorrectRowNo3;
                InteractiveFromDb.CorrectRowNo4 = Interactive.CorrectRowNo4;
                InteractiveFromDb.Image = Interactive.Image;
                InteractiveFromDb.Name1 = Interactive.Name1;
                InteractiveFromDb.Name2 = Interactive.Name2;
                InteractiveFromDb.Name3 = Interactive.Name3;
                InteractiveFromDb.Name4 = Interactive.Name4;
                InteractiveFromDb.Feedback = Interactive.Feedback;
                InteractiveFromDb.LastUpdated = Interactive.LastUpdated;
                InteractiveFromDb.Include = Interactive.Include;
                InteractiveFromDb.CorrectNameListing = Interactive.CorrectNameListing;

                await _db.SaveChangesAsync();
                Message = "Question Updated Successfully!";
                return RedirectToPage("/Questions/Index", new { id = Interactive.AssessmentId });
            }
            else
            {
                Message = "It is not possible to have 2 positions or list names of the same value. Please retry.";
                return RedirectToPage("/Questions/DragnDrop/Edit", new { id = Interactive.Id });
            }
        }

        // Checks for uniqueness between Default values and correct values of each item in the list.
        private Boolean Validator()
        {
            if (Interactive.CorrectRowNo1 != Interactive.CorrectRowNo2
                && Interactive.CorrectRowNo1 != Interactive.CorrectRowNo3
                && Interactive.CorrectRowNo1 != Interactive.CorrectRowNo4
                && Interactive.CorrectRowNo2 != Interactive.CorrectRowNo3
                && Interactive.CorrectRowNo2 != Interactive.CorrectRowNo4
                && Interactive.CorrectRowNo4 != Interactive.CorrectRowNo3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Checks for uniqueness between Default values and correct values of each item in the list.
        private Boolean EntryNameValidator()
        {
            if (Interactive.Name1 != Interactive.Name2
                && Interactive.Name1 != Interactive.Name3
                && Interactive.Name1 != Interactive.Name4
                && Interactive.Name2 != Interactive.Name3
                && Interactive.Name2 != Interactive.Name4
                && Interactive.Name4 != Interactive.Name3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}