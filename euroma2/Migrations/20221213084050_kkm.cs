using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace euroma2.Migrations
{
    /// <inheritdoc />
    public partial class kkm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Date_Range",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_Range", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Food",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Food", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Hipermarket",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Hipermarket", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Stores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Stores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "floorInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modelUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floorInfo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "interests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    group = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reach", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shop",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shopCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ShopInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopInfo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shopSubCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopSubCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Time_Range",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time_Range", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_events_Date_Range_dateRangeid",
                        column: x => x.dateRangeid,
                        principalTable: "Date_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_Date_Range_dateRangeid",
                        column: x => x.dateRangeid,
                        principalTable: "Date_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Floor_Nav_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachedShopNode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorInfoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor_Nav_Point", x => x.id);
                    table.ForeignKey(
                        name: "FK_Floor_Nav_Point_floorInfo_FloorInfoid",
                        column: x => x.FloorInfoid,
                        principalTable: "floorInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LineaShopCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idshop = table.Column<int>(name: "id_shop", type: "int", nullable: false),
                    idcategory = table.Column<int>(name: "id_category", type: "int", nullable: false),
                    Shopid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaShopCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineaShopCategory_shop_Shopid",
                        column: x => x.Shopid,
                        principalTable: "shop",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LineaShopSubCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idshop = table.Column<int>(name: "id_shop", type: "int", nullable: false),
                    idsubcategory = table.Column<int>(name: "id_subcategory", type: "int", nullable: false),
                    Shopid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaShopSubCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineaShopSubCategory_shop_Shopid",
                        column: x => x.Shopid,
                        principalTable: "shop",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "oDay",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<int>(type: "int", nullable: false),
                    hi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shopid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oDay", x => x.id);
                    table.ForeignKey(
                        name: "FK_oDay_shop_Shopid",
                        column: x => x.Shopid,
                        principalTable: "shop",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Floor_Shop",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachedShopid = table.Column<int>(type: "int", nullable: false),
                    FloorInfoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor_Shop", x => x.id);
                    table.ForeignKey(
                        name: "FK_Floor_Shop_ShopInfo_attachedShopid",
                        column: x => x.attachedShopid,
                        principalTable: "ShopInfo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Floor_Shop_floorInfo_FloorInfoid",
                        column: x => x.FloorInfoid,
                        principalTable: "floorInfo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "General",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    globalid = table.Column<int>(type: "int", nullable: false),
                    foodid = table.Column<int>(type: "int", nullable: false),
                    hypermarketid = table.Column<int>(type: "int", nullable: false),
                    ourStoresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_General", x => x.id);
                    table.ForeignKey(
                        name: "FK_General_Day_Opening_Hours_Food_foodid",
                        column: x => x.foodid,
                        principalTable: "Day_Opening_Hours_Food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_General_Day_Opening_Hours_Hipermarket_hypermarketid",
                        column: x => x.hypermarketid,
                        principalTable: "Day_Opening_Hours_Hipermarket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_General_Day_Opening_Hours_Stores_ourStoresid",
                        column: x => x.ourStoresid,
                        principalTable: "Day_Opening_Hours_Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_General_Time_Range_globalid",
                        column: x => x.globalid,
                        principalTable: "Time_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineaInterest",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idevent = table.Column<int>(name: "id_event", type: "int", nullable: false),
                    idinterest = table.Column<int>(name: "id_interest", type: "int", nullable: false),
                    idpromotion = table.Column<int>(name: "id_promotion", type: "int", nullable: false),
                    MallEventid = table.Column<int>(name: "Mall_Eventid", type: "int", nullable: true),
                    Promotionid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaInterest", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineaInterest_events_Mall_Eventid",
                        column: x => x.MallEventid,
                        principalTable: "events",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LineaInterest_promotion_Promotionid",
                        column: x => x.Promotionid,
                        principalTable: "promotion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "opening_hours",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    generalid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opening_hours", x => x.id);
                    table.ForeignKey(
                        name: "FK_opening_hours_General_generalid",
                        column: x => x.generalid,
                        principalTable: "General",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exception_Rules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    globalid = table.Column<int>(type: "int", nullable: false),
                    foodid = table.Column<int>(type: "int", nullable: false),
                    hypermarketid = table.Column<int>(type: "int", nullable: false),
                    ourStoresid = table.Column<int>(type: "int", nullable: false),
                    Openingid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exception_Rules", x => x.id);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_Date_Range_dateRangeid",
                        column: x => x.dateRangeid,
                        principalTable: "Date_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_Day_Opening_Hours_Food_foodid",
                        column: x => x.foodid,
                        principalTable: "Day_Opening_Hours_Food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_Day_Opening_Hours_Hipermarket_hypermarketid",
                        column: x => x.hypermarketid,
                        principalTable: "Day_Opening_Hours_Hipermarket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_Day_Opening_Hours_Stores_ourStoresid",
                        column: x => x.ourStoresid,
                        principalTable: "Day_Opening_Hours_Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_Time_Range_globalid",
                        column: x => x.globalid,
                        principalTable: "Time_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exception_Rules_opening_hours_Openingid",
                        column: x => x.Openingid,
                        principalTable: "opening_hours",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_dateRangeid",
                table: "events",
                column: "dateRangeid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_dateRangeid",
                table: "Exception_Rules",
                column: "dateRangeid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_foodid",
                table: "Exception_Rules",
                column: "foodid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_globalid",
                table: "Exception_Rules",
                column: "globalid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_hypermarketid",
                table: "Exception_Rules",
                column: "hypermarketid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_Openingid",
                table: "Exception_Rules",
                column: "Openingid");

            migrationBuilder.CreateIndex(
                name: "IX_Exception_Rules_ourStoresid",
                table: "Exception_Rules",
                column: "ourStoresid");

            migrationBuilder.CreateIndex(
                name: "IX_Floor_Nav_Point_FloorInfoid",
                table: "Floor_Nav_Point",
                column: "FloorInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_Floor_Shop_attachedShopid",
                table: "Floor_Shop",
                column: "attachedShopid");

            migrationBuilder.CreateIndex(
                name: "IX_Floor_Shop_FloorInfoid",
                table: "Floor_Shop",
                column: "FloorInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_General_foodid",
                table: "General",
                column: "foodid");

            migrationBuilder.CreateIndex(
                name: "IX_General_globalid",
                table: "General",
                column: "globalid");

            migrationBuilder.CreateIndex(
                name: "IX_General_hypermarketid",
                table: "General",
                column: "hypermarketid");

            migrationBuilder.CreateIndex(
                name: "IX_General_ourStoresid",
                table: "General",
                column: "ourStoresid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaInterest_Mall_Eventid",
                table: "LineaInterest",
                column: "Mall_Eventid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaInterest_Promotionid",
                table: "LineaInterest",
                column: "Promotionid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaShopCategory_Shopid",
                table: "LineaShopCategory",
                column: "Shopid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaShopSubCategory_Shopid",
                table: "LineaShopSubCategory",
                column: "Shopid");

            migrationBuilder.CreateIndex(
                name: "IX_oDay_Shopid",
                table: "oDay",
                column: "Shopid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_hours_generalid",
                table: "opening_hours",
                column: "generalid");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_dateRangeid",
                table: "promotion",
                column: "dateRangeid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exception_Rules");

            migrationBuilder.DropTable(
                name: "Floor_Nav_Point");

            migrationBuilder.DropTable(
                name: "Floor_Shop");

            migrationBuilder.DropTable(
                name: "interests");

            migrationBuilder.DropTable(
                name: "LineaInterest");

            migrationBuilder.DropTable(
                name: "LineaShopCategory");

            migrationBuilder.DropTable(
                name: "LineaShopSubCategory");

            migrationBuilder.DropTable(
                name: "oDay");

            migrationBuilder.DropTable(
                name: "reach");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "shopCategory");

            migrationBuilder.DropTable(
                name: "shopSubCategory");

            migrationBuilder.DropTable(
                name: "opening_hours");

            migrationBuilder.DropTable(
                name: "ShopInfo");

            migrationBuilder.DropTable(
                name: "floorInfo");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "shop");

            migrationBuilder.DropTable(
                name: "General");

            migrationBuilder.DropTable(
                name: "Date_Range");

            migrationBuilder.DropTable(
                name: "Day_Opening_Hours_Food");

            migrationBuilder.DropTable(
                name: "Day_Opening_Hours_Hipermarket");

            migrationBuilder.DropTable(
                name: "Day_Opening_Hours_Stores");

            migrationBuilder.DropTable(
                name: "Time_Range");
        }
    }
}
