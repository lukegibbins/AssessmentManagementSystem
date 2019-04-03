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
    public class CreateModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Used to access wwwroot of the project structure
        private readonly IHostingEnvironment _hostingEnvironment;

        // Used to obtain an assessment Id
        public Assessment Assessment { get; set; }

        // Defines answers choices for drop down
        private const string TrueAnswer = "True";
        private const string FalseAnswer = "False";

        // Binds data sent from the create assessment request
        [BindProperty]
        public TrueFalseOption TrueFalse { get; set; }

        // Temp data which confirms database transation. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        // Get current date and time
        public DateTime CurrentDate = DateTime.Now;

        // Constructor
        public CreateModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        // Sets selection options for correct answers
        public List<SelectListItem> AnswerOptions = new List<SelectListItem>()
        {
              new SelectListItem() { Text=TrueAnswer, Value="True"},
              new SelectListItem() { Text=FalseAnswer, Value="False"}
        };

        // Returns the create page to the end user
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get hold of the assessment your creating question for using the Id passed from the index page
            Assessment = await _db.Assessments.FindAsync(id);

            if (Assessment == null)
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

            // Adds the bound Question to the questions table.
            // The new update MCQ has been sent to db.
            // ... We then need to extract out that updated file again to that add an image to it to resave
            _db.Questions.Add(TrueFalse);
            await _db.SaveChangesAsync();

            // Get the web root path of the www.root folder where images are stored.
            // .. this is done through hosting environment
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            // Then, find the question which has just been updated and convert it to MCQ.
            // .. we need it to then re-update with the image
            Question recentlyUpdatedQuestion = _db.Questions.Find(TrueFalse.Id);
            TrueFalseOption convertToTrueFalse = (TrueFalseOption)recentlyUpdatedQuestion;

            // If the file being uploaded is not null and has a value
            if (files[0] != null && files[0].Length > 0)
            {
                // Uploads then references to the images folder in wwwroot
                var uploads = Path.Combine(webRootPath, "images");

                // Get the file extension of file that has been uploaded
                var fileExtension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                // Create a file using the route of the file, the file extension of the uploaded file
                // and the Id of the question and store in the images folder under the name of the QuestionId
                using (var fileStream = new FileStream(Path.Combine(uploads, convertToTrueFalse.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                // Store the same filepath as the property value. E.g  MCQ.image = id+jpg 
                convertToTrueFalse.Image = @"\images\" + convertToTrueFalse.Id + fileExtension;
            }

            // Re-save changes
            await _db.SaveChangesAsync();

            // Confirmation message
            Message = "True/False Question Added Successfully!";

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("/Questions/Index", new { id = TrueFalse.AssessmentId });
        }

    }
}