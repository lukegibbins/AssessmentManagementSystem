using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    public class ShortAnswerOption: Question
    {
        [Display(Name = "Correct answer")]
        public string CorrectAnswer { get; set; }

        [Required]
        public string Feedback { get; set; }
    }
}
