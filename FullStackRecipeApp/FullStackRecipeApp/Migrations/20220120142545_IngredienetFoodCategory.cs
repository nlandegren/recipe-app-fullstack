using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class IngredienetFoodCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredient");

            migrationBuilder.AddColumn<string>(
                name: "FoodCategory",
                table: "Ingredient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodCategory",
                table: "Ingredient");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredient",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
