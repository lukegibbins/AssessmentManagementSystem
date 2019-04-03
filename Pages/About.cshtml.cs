using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveAssessment.Pages
{
    // This means when a user go to http://localhost/about. To access this page, they have to have a role as a teacher
    // This can implemented as a student too.
    [Authorize(Policy = "Teacher")]
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}
