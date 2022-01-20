using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStackRecipeApp.Migrations
{
    public partial class MeasurementNameChangeToUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measurement",
                table: "Measurement");

            migrationBuilder.RenameTable(
                name: "Measurement",
                newName: "Unit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                table: "Unit",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Unit_MeasurementID",
                table: "Quantity",
                column: "MeasurementID",
                principalTable: "Unit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quantity_Unit_MeasurementID",
                table: "Quantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                table: "Unit");

            migrationBuilder.RenameTable(
                name: "Unit",
                newName: "Measurement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measurement",
                table: "Measurement",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quantity_Measurement_MeasurementID",
                table: "Quantity",
                column: "MeasurementID",
                principalTable: "Measurement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
