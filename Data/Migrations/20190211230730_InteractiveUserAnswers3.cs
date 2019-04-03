using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class InteractiveUserAnswers3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Questions",
                newName: "MCQ_Feedback");

            migrationBuilder.AddColumn<string>(
                name: "CorrectNameListing",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectNameListing",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "MCQ_Feedback",
                table: "Questions",
                newName: "Feedback");
        }
    }
}
