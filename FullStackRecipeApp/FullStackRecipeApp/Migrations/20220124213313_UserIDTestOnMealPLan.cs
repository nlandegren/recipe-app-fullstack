using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class UserIDTestOnMealPLan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "MealPlan",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "MealPlan");
        }
    }
}
