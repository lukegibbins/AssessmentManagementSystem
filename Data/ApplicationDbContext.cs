using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveAssessment.Models;
using InteractiveAssessment.Models.QuestionModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InteractiveAssessment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Assessment> Assessments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<UserAnswer> UserAnswers { get; set; }

        public DbSet<UserResult> UserResults { get; set; }

        public DbSet<Statistic> Statistics { get; set; }

        public DbSet<StudentResult> StudentResults { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Need to define models when using inheritance in EF
            builder.Entity<MCQ>();
            builder.Entity<TrueFalseOption>();
            builder.Entity<ShortAnswerOption>();
            builder.Entity<Interactive>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
