using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InteractiveAssessment.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // We are defining additional fields in the register for firstname, surname and accountType
        [Display(Name = "Full Name")]
        public string FirstName { get; set; }
        public string Surname { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " " + Surname; } }
    }
}
