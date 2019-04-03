using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    // Abstract class which represents all questions
    public abstract class Question
    {
        // Common properties to all question types
        [Key]
        public int Id { get; set; }

        public int AssessmentId { get; set; }

        [Display(Name = "Question type")]
        public string QuestionType { get; set; }

        [Required]
        [Display(Name = "Question title")]
        public string QuestionTitle { get; set; }

        [Required]
        [Display(Name = "Last modified")]
        public DateTime LastUpdated { get; set; }

        [Display(Name = "Included")]
        public Boolean Include { get; set; }

        // Used for image uploads
        public string Image { get; set; }

        // This is used when collecting stats on questions
        [Display(Name = "Version No.")]
        [Required]
        public double VersionNo { get; set; }
    }
}
