using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class AssessmentMigrationUpdatedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GetAssessments",
                table: "GetAssessments");

            migrationBuilder.RenameTable(
                name: "GetAssessments",
                newName: "Assessments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "GetAssessments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetAssessments",
                table: "GetAssessments",
                column: "Id");
        }
    }
}
