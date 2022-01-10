using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Ingredient_IngredientID",
                table: "Quantity");

            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "MeasurementID",
                table: "Quantity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IngredientID",
                table: "Quantity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Ingredient_IngredientID",
                table: "Quantity",
                column: "IngredientID",
                principalTable: "Ingredient",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity",
                column: "MeasurementID",
                principalTable: "Measurement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Ingredient_IngredientID",
                table: "Quantity");

            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "MeasurementID",
                table: "Quantity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientID",
                table: "Quantity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Ingredient_IngredientID",
                table: "Quantity",
                column: "IngredientID",
                principalTable: "Ingredient",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity",
                column: "MeasurementID",
                principalTable: "Measurement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
