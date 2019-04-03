using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class TfM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswerBoolean",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TrueFalse_Feedback",
                table: "Questions",
                newName: "TrueFalseOption_Feedback");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Questions",
                newName: "TrueFalseOption_CorrectAnswer");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TrueFalseOption_Feedback",
                table: "Questions",
                newName: "TrueFalse_Feedback");

            migrationBuilder.RenameColumn(
                name: "TrueFalseOption_CorrectAnswer",
                table: "Questions",
                newName: "CorrectAnswer");

            migrationBuilder.AddColumn<bool>(
                name: "CorrectAnswerBoolean",
                table: "Questions",
                nullable: true);
        }
    }
}
