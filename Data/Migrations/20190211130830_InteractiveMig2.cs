using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class InteractiveMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColNo",
                table: "Interactives");

            migrationBuilder.DropColumn(
                name: "CorrectCol",
                table: "Interactives");

            migrationBuilder.DropColumn(
                name: "CorrectRow",
                table: "Interactives");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColNo",
                table: "Interactives",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectCol",
                table: "Interactives",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectRow",
                table: "Interactives",
                nullable: false,
                defaultValue: 0);
        }
    }
}
