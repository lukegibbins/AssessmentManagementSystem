using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    public class Statistic
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Display(Name = "Version No.")]
        [Required]
        public double VersionNo { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public Boolean Pass { get; set; }

        // Constructor
        public Statistic(int questionId, string userId, int assessmentId, Boolean pass, double versionNo)
        {
            QuestionId = questionId;
            UserId = userId;
            AssessmentId = assessmentId;
            Pass = pass;
            VersionNo = versionNo;
        }

        // parameterless constructor
        public Statistic()
        {

        }
    }
}
