using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class UserIDTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Ingredient",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Ingredient");
        }
    }
}
