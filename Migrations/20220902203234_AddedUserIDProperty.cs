using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossFitWOD.Migrations
{
    public partial class AddedUserIDProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WODs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WODs");
        }
    }
}
