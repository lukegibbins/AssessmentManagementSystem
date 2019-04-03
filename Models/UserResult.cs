using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{

    // Defines the score the user achieved for an assessment with the assessment name and date taken
    public class UserResult
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        [Display(Name = "Assessment name")]
        public string AssessmentName { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        [Display(Name = "Date completed")]
        public DateTime DateTaken { get; set; }
    }
}
