using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace InteractiveAssessment.Models
{
    // Model for assessment
    public class Assessment
    {
        [Required]
        public int Id { get; set; }

        // Acts as the foreign key to the AspNetUsers table
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display (Name = "Assessment name")]
        public string AssessmentName { get; set; }

        [Required]
        [Display(Name = "Last modified")]
        public DateTime LastUpdated { get; set; }

        [Display(Name = "Active status")]
        public string ActiveStatus { get; set; }

        // Unique key for assessment password
        [Required]
        [Display(Name = "Password")]
        [Index(IsUnique = true)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Duration (mins)")]
        public int Duration { get; set; }
    }
}
