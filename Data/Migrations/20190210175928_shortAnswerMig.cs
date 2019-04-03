using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class shortAnswerMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Questions",
                newName: "ShortAnswerOption_Feedback");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Questions",
                newName: "ShortAnswerOption_CorrectAnswer");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
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
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ShortAnswerOption_Feedback",
                table: "Questions",
                newName: "Feedback");

            migrationBuilder.RenameColumn(
                name: "ShortAnswerOption_CorrectAnswer",
                table: "Questions",
                newName: "CorrectAnswer");
        }
    }
}
