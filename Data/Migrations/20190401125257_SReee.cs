using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class SReee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StudentResults",
                newName: "StudentUserId");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "StudentResults",
                newName: "StudentScore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentUserId",
                table: "StudentResults",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "StudentScore",
                table: "StudentResults",
                newName: "Score");
        }
    }
}
