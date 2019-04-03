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
    public class CreateModel : PageModel
    {
        // Accessing database instance 
        private readonly ApplicationDbContext _db;

        // Used to access wwwroot of the project structure
        private readonly IHostingEnvironment _hostingEnvironment;

        // Used to obtain an assessment Id
        public Assessment Assessment { get; set; }

        // Binds data sent from the create assessment request
        [BindProperty]
        public Interactive Interactive { get; set; }

        // Temp data which confirms database transation. Disposed on URL change
        [TempData]
        public string Message { get; set; }

        // Get current date and time
        public DateTime CurrentDate = DateTime.Now;

        // Options for RowNo and CorrectRowNo
        public SelectList AnswerOptions { get; set; }
        List<int> IntValues = new List<int> { 1, 2, 3, 4 };

        // Constructor
        public CreateModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            AnswerOptions = new SelectList(IntValues);
        }

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

            // String builder. They will build a string which is used to display to the end user for the results
            // .. section.
            string[] StringBuilder = new string[4];
            StringBuilder[Interactive.CorrectRowNo1 - 1] = Interactive.Name1;
            StringBuilder[Interactive.CorrectRowNo2 - 1] = Interactive.Name2;
            StringBuilder[Interactive.CorrectRowNo3 - 1] = Interactive.Name3;
            StringBuilder[Interactive.CorrectRowNo4 - 1] = Interactive.Name4;

            Interactive.CorrectNameListing = StringBuilder[0] 
                + ", " + StringBuilder[1] + ", " + StringBuilder[2] + ", " + StringBuilder[3];

            if (Validator() == true && EntryNameValidator() == true)
            {
                _db.Questions.Add(Interactive);
                await _db.SaveChangesAsync();
            }
            else
            {
                Message = "It is not possible to have 2 positions or list names of the same value. Please retry.";
                return RedirectToPage("/Questions/DragnDrop/Create", new { id = Interactive.AssessmentId });
            }

            // Get the web root path of the www.root folder where images are stored.
            // .. this is done through hosting environment
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            // Then, find the question which has just been updated and convert it to MCQ.
            // .. we need it to then re-update with the image
            Question recentlyUpdatedQuestion = _db.Questions.Find(Interactive.Id);
            Interactive convertToInteractive = (Interactive)recentlyUpdatedQuestion;

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
                using (var fileStream = new FileStream(Path.Combine(uploads, convertToInteractive.Id + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                // Store the same filepath as the property value. E.g  MCQ.image = id+jpg 
                convertToInteractive.Image = @"\images\" + convertToInteractive.Id + fileExtension;
            }

            //convertToInteractive.CorrectNameListing = 

            // Re-save changes
            await _db.SaveChangesAsync();

            // Confirmation message
            Message = "Interactive (List-Sort) Question Added Successfully!";

            // This returns to the index page within the current folder /Assessments
            return RedirectToPage("/Questions/Index", new { id = convertToInteractive.AssessmentId });
        }


        // Checks for uniqueness between Default values and correct values of each item in the list.
        private Boolean Validator()
        {
            if(Interactive.CorrectRowNo1 != Interactive.CorrectRowNo2
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