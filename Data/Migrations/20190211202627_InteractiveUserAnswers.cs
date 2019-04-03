using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class InteractiveUserAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InteractiveName1",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteractiveName2",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteractiveName4",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterativeName3",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNo1",
                table: "UserAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNo2",
                table: "UserAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNo3",
                table: "UserAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNo4",
                table: "UserAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "hasInteractiveBeenAnswered",
                table: "UserAnswers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractiveName1",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "InteractiveName2",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "InteractiveName4",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "InterativeName3",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RowNo1",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RowNo2",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RowNo3",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RowNo4",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "hasInteractiveBeenAnswered",
                table: "UserAnswers");
        }
    }
}
