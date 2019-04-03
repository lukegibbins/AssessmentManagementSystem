using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveAssessment.Data.Migrations
{
    public partial class InteractiveMig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interactives");

            migrationBuilder.AddColumn<int>(
                name: "CorrectRowNo1",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectRowNo2",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectRowNo3",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectRowNo4",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name1",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name2",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name3",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name4",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNo1",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNo2",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNo3",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNo4",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectRowNo1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectRowNo2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectRowNo3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectRowNo4",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name4",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RowNo1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RowNo2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RowNo3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RowNo4",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "Interactives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorrectRowNo = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RowNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactives", x => x.Id);
                });
        }
    }
}
