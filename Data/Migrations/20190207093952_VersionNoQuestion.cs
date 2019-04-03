using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class VersionNoQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "VersionNo",
                table: "Questions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "ActiveStatus",
                table: "Assessments",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionNo",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "ActiveStatus",
                table: "Assessments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
