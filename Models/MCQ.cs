using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models.QuestionModels
{
    // Model for multiple choice question
    public class MCQ : Question
    {
        [Display(Name = "Answer choice 1")]
        [Required]
        public string Answer1 { get; set; }
        
        [Display(Name = "Answer choice 2")]
        [Required]
        public string Answer2 { get; set; }

        [Display(Name = "Answer choice 3")]
        [Required]
        public string Answer3 { get; set; }

        [Display(Name = "Answer choice 4")]
        [Required]
        public string Answer4 { get; set; }

        // This stores the selection of the drop down list
        [Display(Name = "Correct answer")]
        [Required]
        public string CorrectAnswer { get; set; }

        // This then stores the value from the drop down list
        public string CorrectAnswerStringValue { get; set; }

        [Display(Name = "Add constructive feedback")]
        [Required]
        public string Feedback { get; set; }
    }
}
