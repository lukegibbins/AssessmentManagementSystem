using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class Inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MCQs",
                table: "MCQs");

            migrationBuilder.RenameTable(
                name: "MCQs",
                newName: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Questions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "MCQs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MCQs",
                table: "MCQs",
                column: "Id");
        }
    }
}
