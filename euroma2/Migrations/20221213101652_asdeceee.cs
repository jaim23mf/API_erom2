using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace euroma2.Migrations
{
    /// <inheritdoc />
    public partial class asdeceee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_Date_Range_dateRangeid",
                table: "Exception_Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Food_foodid",
                table: "Exception_Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "Exception_Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Stores_ourStoresid",
                table: "Exception_Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_Time_Range_globalid",
                table: "Exception_Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Exception_Rules_opening_hours_Openingid",
                table: "Exception_Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exception_Rules",
                table: "Exception_Rules");

            migrationBuilder.RenameTable(
                name: "Exception_Rules",
                newName: "opening_exceptions");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_ourStoresid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_ourStoresid");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_Openingid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_Openingid");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_hypermarketid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_hypermarketid");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_globalid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_globalid");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_foodid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_foodid");

            migrationBuilder.RenameIndex(
                name: "IX_Exception_Rules_dateRangeid",
                table: "opening_exceptions",
                newName: "IX_opening_exceptions_dateRangeid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_opening_exceptions",
                table: "opening_exceptions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_Date_Range_dateRangeid",
                table: "opening_exceptions",
                column: "dateRangeid",
                principalTable: "Date_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Food_foodid",
                table: "opening_exceptions",
                column: "foodid",
                principalTable: "Day_Opening_Hours_Food",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "opening_exceptions",
                column: "hypermarketid",
                principalTable: "Day_Opening_Hours_Hipermarket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Stores_ourStoresid",
                table: "opening_exceptions",
                column: "ourStoresid",
                principalTable: "Day_Opening_Hours_Stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_Time_Range_globalid",
                table: "opening_exceptions",
                column: "globalid",
                principalTable: "Time_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_exceptions_opening_hours_Openingid",
                table: "opening_exceptions",
                column: "Openingid",
                principalTable: "opening_hours",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_Date_Range_dateRangeid",
                table: "opening_exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Food_foodid",
                table: "opening_exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "opening_exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_Day_Opening_Hours_Stores_ourStoresid",
                table: "opening_exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_Time_Range_globalid",
                table: "opening_exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_exceptions_opening_hours_Openingid",
                table: "opening_exceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_opening_exceptions",
                table: "opening_exceptions");

            migrationBuilder.RenameTable(
                name: "opening_exceptions",
                newName: "Exception_Rules");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_ourStoresid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_ourStoresid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_Openingid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_Openingid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_hypermarketid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_hypermarketid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_globalid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_globalid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_foodid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_foodid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_exceptions_dateRangeid",
                table: "Exception_Rules",
                newName: "IX_Exception_Rules_dateRangeid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exception_Rules",
                table: "Exception_Rules",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_Date_Range_dateRangeid",
                table: "Exception_Rules",
                column: "dateRangeid",
                principalTable: "Date_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Food_foodid",
                table: "Exception_Rules",
                column: "foodid",
                principalTable: "Day_Opening_Hours_Food",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "Exception_Rules",
                column: "hypermarketid",
                principalTable: "Day_Opening_Hours_Hipermarket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_Day_Opening_Hours_Stores_ourStoresid",
                table: "Exception_Rules",
                column: "ourStoresid",
                principalTable: "Day_Opening_Hours_Stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_Time_Range_globalid",
                table: "Exception_Rules",
                column: "globalid",
                principalTable: "Time_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exception_Rules_opening_hours_Openingid",
                table: "Exception_Rules",
                column: "Openingid",
                principalTable: "opening_hours",
                principalColumn: "id");
        }
    }
}
