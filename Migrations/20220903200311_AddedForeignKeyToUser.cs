using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossFitWOD.Migrations
{
    public partial class AddedForeignKeyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "WODs");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "WODs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WODs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WODs_UserId",
                table: "WODs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WODs_AspNetUsers_UserId",
                table: "WODs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WODs_AspNetUsers_UserId",
                table: "WODs");

            migrationBuilder.DropIndex(
                name: "IX_WODs_UserId",
                table: "WODs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WODs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "WODs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "WODs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
