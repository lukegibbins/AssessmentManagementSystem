using InteractiveAssessment.Models.QuestionModels;
using InteractiveAssessment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveAssessment.Data
{
    public class DataSeeder
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManger = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();

            // Defines roles
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole
                {
                    Id = "1fa12521-564e-4a90-b6fe-c150ae2774b1",
                    ConcurrencyStamp = "72115431-db0a-4474-bf9d-667f4adf8e25",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                });

                context.Roles.Add(new IdentityRole
                {
                    Id = "4425affe-42de-4638-bef1-3cc2b8535b28",
                    ConcurrencyStamp = "85163605-b746-4f5a-8b0c-26ece841f837",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                });

                context.SaveChanges();
            }

            // Creates users
            if (!context.Users.Any())
            {
                ApplicationUser teacherUser = new ApplicationUser()
                {
                    Email = "teacher@sunderland.ac.uk",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "teacher@sunderland.ac.uk",
                    FirstName = "Michael",
                    Surname = "Scott",
                    AccountType = "Teacher"
                };
                userManger.CreateAsync(teacherUser, "Teacher1!");

                ApplicationUser studentUser = new ApplicationUser()
                {
                    Email = "student@sunderland.ac.uk",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "student@sunderland.ac.uk",
                    FirstName = "Dwight",
                    Surname = "Schrute",
                    AccountType = "Student"
                };
                userManger.CreateAsync(studentUser, "Student1!");


                // Add users to user roles
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = teacherUser.Id,
                    RoleId = "1fa12521-564e-4a90-b6fe-c150ae2774b1"
                });

                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = studentUser.Id,
                    RoleId = "4425affe-42de-4638-bef1-3cc2b8535b28"
                });

                context.SaveChanges();
            }

            // Create assessment
            if (!context.Assessments.Any())
            {
                // Obtain UserId
                var user = userManger.Users.SingleOrDefault(c => c.UserName == "teacher@sunderland.ac.uk");

                // Create Assessment
                Assessment assessment = new Assessment();
                assessment.UserId = user.Id.ToString();
                assessment.ActiveStatus = "Active";
                assessment.AssessmentName = "C# Software Development Test (Intermediate)";
                assessment.Duration = 6;
                assessment.LastUpdated = DateTime.Now;
                assessment.Password = "ZN66XRT3";
                context.Assessments.AddAsync(assessment);
                context.SaveChanges();

                var assessmentFromDb = context.Assessments.SingleOrDefault(c => c.Password == "ZN66XRT3");

                // Create MCQ
                MCQ MCQ = new MCQ();
                MCQ.AssessmentId = assessmentFromDb.Id;
                MCQ.QuestionType = "MCQ";
                MCQ.LastUpdated = DateTime.Now;
                MCQ.Include = true;
                MCQ.Image = @"\images\Q1.JPG";
                MCQ.VersionNo = 1;
                MCQ.QuestionTitle = "Analyse the image and select the colour that is output to the console";
                MCQ.Answer1 = "green";
                MCQ.Answer2 = "orange";
                MCQ.Answer3 = "blue";
                MCQ.Answer4 = "red";
                MCQ.CorrectAnswer = "Answer choice 4";
                MCQ.CorrectAnswerStringValue = "red";
                MCQ.Feedback = "The console would output array element 2, which is the 3rd item in the list. " +
                    "Visit https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/ for more information on C# arrays.";

                //Create True/False
                TrueFalseOption TrueFalse = new TrueFalseOption();
                TrueFalse.AssessmentId = assessmentFromDb.Id;
                TrueFalse.QuestionType = "True/False";
                TrueFalse.LastUpdated = DateTime.Now;
                TrueFalse.Include = true;
                TrueFalse.Image = null;
                TrueFalse.VersionNo = 1;
                TrueFalse.QuestionTitle = "An abstract class can be instantiated";
                TrueFalse.CorrectAnswer = "False";
                TrueFalse.Feedback = "Abstract class cannot be instantiated. However, any class that inherits from an abstract class" +
                    "can be instantiated. Visit https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract for more information on abstract classes";

                //Create Short Answer
                ShortAnswerOption ShortAnswer = new ShortAnswerOption();
                ShortAnswer.AssessmentId = assessmentFromDb.Id;
                ShortAnswer.QuestionType = "Short Answer";
                ShortAnswer.LastUpdated = DateTime.Now;
                ShortAnswer.Include = true;
                ShortAnswer.Image = null;
                ShortAnswer.VersionNo = 1;
                ShortAnswer.QuestionTitle = "What is the return type applied to a method thats returns no value?";
                ShortAnswer.CorrectAnswer = "Void";
                ShortAnswer.Feedback = "A method which returns nothing has a return type of Void. E.g public void MyMethod(). " +
                    "Visit https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/void for more info on the void return type.";


                // Interative
                Interactive Interactive = new Interactive();
                Interactive.AssessmentId = assessmentFromDb.Id;
                Interactive.QuestionType = "Interactive";
                Interactive.LastUpdated = DateTime.Now;
                Interactive.Include = true;
                Interactive.Image = @"\images\Q4.JPG";
                Interactive.VersionNo = 1;
                Interactive.QuestionTitle = "Analyse the image below and sort the names in order of their generated value (highest first)";
                Interactive.Name1 = "Stuart";
                Interactive.RowNo1 = 1;
                Interactive.CorrectRowNo1 = 2;
                Interactive.Name2 = "Luke";
                Interactive.RowNo2 = 2;
                Interactive.CorrectRowNo2 = 3;
                Interactive.Name3 = "Andrew";
                Interactive.RowNo3 = 3;
                Interactive.CorrectRowNo3 = 1;
                Interactive.Name4 = "John";
                Interactive.RowNo4 = 4;
                Interactive.CorrectRowNo4 = 4;
                Interactive.CorrectNameListing = "Andrew, Stuart, Luke, John";
                Interactive.Feedback = "Andrew scores 8, Stuart scores 7, Luke scores 6 and John scores 2.";

                // Create MCQ
                MCQ MCQ2 = new MCQ();
                MCQ2.AssessmentId = assessmentFromDb.Id;
                MCQ2.QuestionType = "MCQ";
                MCQ2.LastUpdated = DateTime.Now;
                MCQ2.Include = true;
                MCQ2.Image = null;
                MCQ2.VersionNo = 1;
                MCQ2.QuestionTitle = "A variable declared inside of a method is a _____";
                MCQ2.Answer1 = "Serial variable";
                MCQ2.Answer2 = "Local variable";
                MCQ2.Answer3 = "Private variable";
                MCQ2.Answer4 = "Static variable";
                MCQ2.CorrectAnswer = "Answer choice 2";
                MCQ2.CorrectAnswerStringValue = "Local variable";
                MCQ2.Feedback = "Variables declared inside of method are local variables.";

                //Create mcq
                MCQ MCQ3 = new MCQ();
                MCQ3.AssessmentId = assessmentFromDb.Id;
                MCQ3.QuestionType = "MCQ";
                MCQ3.LastUpdated = DateTime.Now;
                MCQ3.Include = true;
                MCQ3.Image = @"\images\divisibleBy2.JPG";
                MCQ3.VersionNo = 1;
                MCQ3.QuestionTitle = "Analyse the image and select the numbers that are output to the console in their correct order";
                MCQ3.Answer1 = "97, 34, 23";
                MCQ3.Answer2 = "4, 76, 34";
                MCQ3.Answer3 = "34, 4";
                MCQ3.Answer4 = "4, 23, 97, 76";
                MCQ3.CorrectAnswer = "Answer choice 2";
                MCQ3.CorrectAnswerStringValue = "4, 76, 34";
                MCQ3.Feedback = "The answer is 4, 76, 34. The foreach loop iterates through the list and output any values that are divisible by 2.";

                // Interative
                Interactive Interactive2 = new Interactive();
                Interactive2.AssessmentId = assessmentFromDb.Id;
                Interactive2.QuestionType = "Interactive";
                Interactive2.LastUpdated = DateTime.Now;
                Interactive2.Include = true;
                Interactive2.Image = @"\images\i2.JPG";
                Interactive2.VersionNo = 1;
                Interactive2.QuestionTitle = "Analyse the image below and arrange these numbers in order of the output";
                Interactive2.Name1 = "33";
                Interactive2.RowNo1 = 1;
                Interactive2.CorrectRowNo1 = 3;
                Interactive2.Name2 = "23";
                Interactive2.RowNo2 = 2;
                Interactive2.CorrectRowNo2 = 4;
                Interactive2.Name3 = "45";
                Interactive2.RowNo3 = 3;
                Interactive2.CorrectRowNo3 = 2;
                Interactive2.Name4 = "65";
                Interactive2.RowNo4 = 4;
                Interactive2.CorrectRowNo4 = 1;
                Interactive2.CorrectNameListing = "65, 45, 33, 23";
                Interactive2.Feedback = "The list is sorted firstly in ascending order and then re-sorted or reversed into descending order. " +
                    "The correct value is then 65, 45, 33, 23.";


                //Create Short Answer
                ShortAnswerOption ShortAnswer2 = new ShortAnswerOption();
                ShortAnswer2.AssessmentId = assessmentFromDb.Id;
                ShortAnswer2.QuestionType = "Short Answer";
                ShortAnswer2.LastUpdated = DateTime.Now;
                ShortAnswer2.Include = true;
                ShortAnswer2.Image = null;
                ShortAnswer2.VersionNo = 1;
                ShortAnswer2.QuestionTitle = "What is the name given to a method of a class which gets invoked whenever an instance of that class is created?";
                ShortAnswer2.CorrectAnswer = "Constructor";
                ShortAnswer2.Feedback = "Constructor. A constructor is a special method of the class which gets automatically invoked whenever an instance of the class is created." +
                    " A class may have multiple constructors that take different arguments. Visit https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors for more information.";


                context.Questions.AddRangeAsync(MCQ, ShortAnswer, Interactive, TrueFalse, ShortAnswer2, Interactive2, MCQ2, MCQ3);
                context.SaveChanges();
            }
        }
    }
}
//method of the class which gets automatically invoked whenever an instance of the class is created