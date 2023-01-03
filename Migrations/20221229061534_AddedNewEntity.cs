using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossFitWOD.Migrations
{
    public partial class AddedNewEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WODs_AspNetUsers_UserId",
                table: "WODs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WODs",
                table: "WODs");

            migrationBuilder.EnsureSchema(
                name: "Workouts");

            migrationBuilder.RenameTable(
                name: "WODs",
                newName: "CrossFit",
                newSchema: "Workouts");

            migrationBuilder.RenameIndex(
                name: "IX_WODs_UserId",
                schema: "Workouts",
                table: "CrossFit",
                newName: "IX_CrossFit_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrossFit",
                schema: "Workouts",
                table: "CrossFit",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Strength",
                schema: "Workouts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strength", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CrossFit_AspNetUsers_UserId",
                schema: "Workouts",
                table: "CrossFit",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrossFit_AspNetUsers_UserId",
                schema: "Workouts",
                table: "CrossFit");

            migrationBuilder.DropTable(
                name: "Strength",
                schema: "Workouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrossFit",
                schema: "Workouts",
                table: "CrossFit");

            migrationBuilder.RenameTable(
                name: "CrossFit",
                schema: "Workouts",
                newName: "WODs");

            migrationBuilder.RenameIndex(
                name: "IX_CrossFit_UserId",
                table: "WODs",
                newName: "IX_WODs_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WODs",
                table: "WODs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WODs_AspNetUsers_UserId",
                table: "WODs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
