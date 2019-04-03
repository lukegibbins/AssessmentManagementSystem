using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Models
{
    public class UserAnswer
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        public string AnswerGiven { get; set; }

        public int AnswerPosition { get; set; }

        // Interactive properties
        public Boolean hasInteractiveBeenAnswered { get; set; }

        // Only used to replay back to the user for checking answers and reviewing results.
        public string InteractiveName1 { get; set; }
        public string InteractiveName2 { get; set; }
        public string InteractiveName3 { get; set; }
        public string InteractiveName4 { get; set; }

        // If (UserAnswer.RowNo1 == Question.CorrectRowNo1)... and the rest => {Correct}
        public int RowNo1 { get; set; } 
        public int RowNo2 { get; set; }
        public int RowNo3 { get; set; }
        public int RowNo4 { get; set; }
    }
}
