using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class UserIDOnUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Unit",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Unit");
        }
    }
}
