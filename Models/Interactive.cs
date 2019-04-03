using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    public class Interactive: Question
    {
        // Represents the names for the sortable list
        [Display(Name ="List Item 1")]
        [Required]
        public string Name1 { get; set; }

        [Display(Name = "List Item 2")]
        [Required]
        public string Name2 { get; set; }

        [Display(Name = "List Item 3")]
        [Required]
        public string Name3 { get; set; }

        [Display(Name = "List Item 4")]
        [Required]
        public string Name4 { get; set; }

        // Represents the default positions of the sortable list
        [Display(Name = "Default Position")]
        [Required]
        public int RowNo1 { get; set; }

        [Display(Name = "Default Position")]
        [Required]
        public int RowNo2 { get; set; }

        [Display(Name = "Default Position")]
        [Required]
        public int RowNo3 { get; set; }

        [Display(Name = "Default Position")]
        [Required]
        public int RowNo4 { get; set; }

        // Represents the correct positions of the sortable list
        [Display(Name = "Correct Position")]
        [Required]
        public int CorrectRowNo1 { get; set; }

        [Display(Name = "Correct Position")]
        [Required]
        public int CorrectRowNo2 { get; set; }

        [Display(Name = "Correct Position")]
        [Required]
        public int CorrectRowNo3 { get; set; }

        [Display(Name = "Correct Position")]
        [Required]
        public int CorrectRowNo4 { get; set; }

        // A string of the correct names. E.g Luke, John, Billy, Tommy to represent 1, 2, 3, 4
        public string CorrectNameListing { get; set; }

        // Feedback
        [Required]
        public string Feedback { get; set; }
    }
}
