using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class MCQtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MCQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer1 = table.Column<string>(nullable: false),
                    Answer2 = table.Column<string>(nullable: false),
                    Answer3 = table.Column<string>(nullable: false),
                    Answer4 = table.Column<string>(nullable: false),
                    AssessmentId = table.Column<int>(nullable: false),
                    CorrectAnswer = table.Column<string>(nullable: false),
                    CorrectAnswerStringValue = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: false),
                    QuestionTitle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCQs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MCQs");
        }
    }
}
