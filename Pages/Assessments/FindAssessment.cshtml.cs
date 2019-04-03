using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Pages
{
    public class FindAssessmentModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Assessment Assessment { get; set; }
        public int QuestionCount { get; set; }

        public List<SelectListItem> AssessmentIds;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public FindAssessment _findAssessment { get; set; }

        public FindAssessmentModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            AssessmentIds = new List<SelectListItem>();
            var assessmentList = await _db.Assessments.ToListAsync();
           
            if(assessmentList.Count > 0)
            {
                foreach(var x in assessmentList)
                {
                    AssessmentIds.Add(new SelectListItem() { Text = x.Id.ToString(), Value = x.Id.ToString() });
                }
            }
            else
            {
                AssessmentIds.Add(new SelectListItem() { Text = "N/A", Value = "0"});
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Find 1 assessment using the users supplied password
            Assessment = await _db.Assessments.FirstOrDefaultAsync(
                x => x.Password == _findAssessment._password && x.Id == int.Parse(_findAssessment._id) && x.ActiveStatus == "Active");

            if (Assessment != null)
            {
                // Passes through the Id of the assessment found from the user password to 
                // .. the AssessmentDetails page.
                QuestionCount = _db.Questions.Where(c => c.AssessmentId == Assessment.Id).Count();
                if (QuestionCount == 0)
                {
                    Message = "There are no questions set for this assessment. Please contact your teacher for assistance.";
                    return Page();
                }

                return RedirectToPage("/Assessments/AssessmentBrief", new { id = Assessment.Id });
            }

            Message = "Unable to find assessment. Please retry or contact your teacher for assistance.";

            // Re-populate list
            AssessmentIds = new List<SelectListItem>();
            var assessmentList = await _db.Assessments.ToListAsync();
            if (assessmentList.Count > 0)
            {
                foreach (var x in assessmentList)
                {
                    AssessmentIds.Add(new SelectListItem() { Text = x.Id.ToString(), Value = x.Id.ToString() });
                }
            }
            else
            {
                AssessmentIds.Add(new SelectListItem() { Text = "N/A", Value = "0" });
            }

            return Page();
        }
    }

    public class FindAssessment
    {
        public string _id { get; set; }
        public string _password { get; set; }
    }

 
}