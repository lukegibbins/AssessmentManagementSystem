using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class TF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Questions",
                newName: "TrueFalse_Feedback");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CorrectAnswerBoolean",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswerBoolean",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TrueFalse_Feedback",
                table: "Questions",
                newName: "Feedback");
        }
    }
}
