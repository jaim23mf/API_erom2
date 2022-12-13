using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace euroma2.Migrations
{
    /// <inheritdoc />
    public partial class ad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_General_Day_Opening_Hours_Food_foodid",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_General_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_General_Day_Opening_Hours_Stores_ourStoresid",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_General_Time_Range_globalid",
                table: "General");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_hours_General_generalid",
                table: "opening_hours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_General",
                table: "General");

            migrationBuilder.RenameTable(
                name: "General",
                newName: "opening_general");

            migrationBuilder.RenameIndex(
                name: "IX_General_ourStoresid",
                table: "opening_general",
                newName: "IX_opening_general_ourStoresid");

            migrationBuilder.RenameIndex(
                name: "IX_General_hypermarketid",
                table: "opening_general",
                newName: "IX_opening_general_hypermarketid");

            migrationBuilder.RenameIndex(
                name: "IX_General_globalid",
                table: "opening_general",
                newName: "IX_opening_general_globalid");

            migrationBuilder.RenameIndex(
                name: "IX_General_foodid",
                table: "opening_general",
                newName: "IX_opening_general_foodid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_opening_general",
                table: "opening_general",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Food_foodid",
                table: "opening_general",
                column: "foodid",
                principalTable: "Day_Opening_Hours_Food",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "opening_general",
                column: "hypermarketid",
                principalTable: "Day_Opening_Hours_Hipermarket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Stores_ourStoresid",
                table: "opening_general",
                column: "ourStoresid",
                principalTable: "Day_Opening_Hours_Stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_general_Time_Range_globalid",
                table: "opening_general",
                column: "globalid",
                principalTable: "Time_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_hours_opening_general_generalid",
                table: "opening_hours",
                column: "generalid",
                principalTable: "opening_general",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Food_foodid",
                table: "opening_general");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "opening_general");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_general_Day_Opening_Hours_Stores_ourStoresid",
                table: "opening_general");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_general_Time_Range_globalid",
                table: "opening_general");

            migrationBuilder.DropForeignKey(
                name: "FK_opening_hours_opening_general_generalid",
                table: "opening_hours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_opening_general",
                table: "opening_general");

            migrationBuilder.RenameTable(
                name: "opening_general",
                newName: "General");

            migrationBuilder.RenameIndex(
                name: "IX_opening_general_ourStoresid",
                table: "General",
                newName: "IX_General_ourStoresid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_general_hypermarketid",
                table: "General",
                newName: "IX_General_hypermarketid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_general_globalid",
                table: "General",
                newName: "IX_General_globalid");

            migrationBuilder.RenameIndex(
                name: "IX_opening_general_foodid",
                table: "General",
                newName: "IX_General_foodid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_General",
                table: "General",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_General_Day_Opening_Hours_Food_foodid",
                table: "General",
                column: "foodid",
                principalTable: "Day_Opening_Hours_Food",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_General_Day_Opening_Hours_Hipermarket_hypermarketid",
                table: "General",
                column: "hypermarketid",
                principalTable: "Day_Opening_Hours_Hipermarket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_General_Day_Opening_Hours_Stores_ourStoresid",
                table: "General",
                column: "ourStoresid",
                principalTable: "Day_Opening_Hours_Stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_General_Time_Range_globalid",
                table: "General",
                column: "globalid",
                principalTable: "Time_Range",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opening_hours_General_generalid",
                table: "opening_hours",
                column: "generalid",
                principalTable: "General",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
