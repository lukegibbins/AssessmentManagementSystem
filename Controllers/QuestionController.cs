using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Data;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveAssessment.Pages.Questions
{
    // This controller essentially routes the URI depending on the question type
    // .. being edited. E.g (MCQ or image)
    public class QuestionController : Controller
    {
        public Question Question;
        private readonly ApplicationDbContext _db;

        public QuestionController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Defines the route for the Edit Action { controller/action/id }
        [Route("Question/Edit{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            // Check the id of the question being selected for editing
            if (id == null)
            {
                return NotFound();
            }

            // Get hold of the Question
            Question = await _db.Questions.FindAsync(id);

            // If question does not have a value, return not found
            if (Question == null)
            {
                return NotFound();
            }

            // If the question selected to edit is a multiple choice question, then
            // .. redirect user to an edit page for a multiple choice question.
            // Will add more question types here later...
            if (Question.QuestionType == "MCQ")
            {
                return RedirectToPage("/Questions/MultipleChoice/Edit", new { id = Question.Id });
            }
            else if (Question.QuestionType == "True/False")
            {
                return RedirectToPage("/Questions/TrueFalse/Edit", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Short Answer")
            {
                return RedirectToPage("/Questions/ShortAnswer/Edit", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Interactive")
            {
                return RedirectToPage("/Questions/DragnDrop/Edit", new { id = Question.Id });
            }
            else
            {
                return NotFound();
            }
        }

        // Defines the route for the Edit Action { controller/action/id }
        [Route("Question/Delete{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            // Check the id of the question being selected for editing
            if (id == null)
            {
                return NotFound();
            }

            // Get hold of the Question
            Question = await _db.Questions.FindAsync(id);

            // If question does not have a value, return not found
            if (Question == null)
            {
                return NotFound();
            }

            // If the question selected to edit is a multiple choice question, then
            // .. redirect user to an edit page for a multiple choice question.
            // Will add more question types here later...
            if (Question.QuestionType == "MCQ")
            {
                return RedirectToPage("/Questions/MultipleChoice/Delete", new { id = Question.Id });
            }
            else if (Question.QuestionType == "True/False")
            {
                return RedirectToPage("/Questions/TrueFalse/Delete", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Short Answer")
            {
                return RedirectToPage("/Questions/ShortAnswer/Delete", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Interactive")
            {
                return RedirectToPage("/Questions/DragnDrop/Delete", new { id = Question.Id });
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Question/Details{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            // Check the id of the question being selected for editing
            if (id == null)
            {
                return NotFound();
            }

            // Get hold of the Question
            Question = await _db.Questions.FindAsync(id);

            // If question does not have a value, return not found
            if (Question == null)
            {
                return NotFound();
            }

            // If the question selected to edit is a multiple choice question, then
            // .. redirect user to an edit page for a multiple choice question.
            // Will add more question types here later...
            if (Question.QuestionType == "MCQ")
            {
                return RedirectToPage("/Questions/MultipleChoice/Details", new { id = Question.Id });
            }
            else if (Question.QuestionType == "True/False")
            {
                return RedirectToPage("/Questions/TrueFalse/Details", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Short Answer")
            {
                return RedirectToPage("/Questions/ShortAnswer/Details", new { id = Question.Id });
            }
            else if (Question.QuestionType == "Interactive")
            {
                return RedirectToPage("/Questions/DragnDrop/Details", new { id = Question.Id });
            }
            else
            {
                return NotFound();
            }
        }
    }
}