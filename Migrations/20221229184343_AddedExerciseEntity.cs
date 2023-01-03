using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossFitWOD.Migrations
{
    public partial class AddedExerciseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Workouts",
                table: "Strength");

            migrationBuilder.RenameColumn(
                name: "ExerciseName",
                schema: "Workouts",
                table: "Strength",
                newName: "ExerciseId");

            migrationBuilder.CreateTable(
                name: "Exercises",
                schema: "Workouts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises",
                schema: "Workouts");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                schema: "Workouts",
                table: "Strength",
                newName: "ExerciseName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Workouts",
                table: "Strength",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
