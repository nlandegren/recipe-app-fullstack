using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class AddedCourseAndDifficulty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseCategory",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Quantity",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseCategory",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Recipe");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Quantity",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
