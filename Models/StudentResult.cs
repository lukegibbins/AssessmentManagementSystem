using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    public class StudentResult
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string StudentUserId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        [Display(Name = "Assessment name")]
        public string AssessmentName { get; set; }

        [Required]
        [Display(Name = "My score")]
        public int StudentScore { get; set; }

        [Required]
        [Display(Name = "Date completed")]
        public DateTime DateTaken { get; set; }
    }
}
