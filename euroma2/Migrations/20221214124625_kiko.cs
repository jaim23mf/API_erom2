using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace euroma2.Migrations
{
    public partial class kiko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Date_Range",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_Range", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Food",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Food", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Hipermarket",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Hipermarket", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Day_Opening_Hours_Stores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromWeekDay = table.Column<int>(type: "int", nullable: false),
                    toWeekDay = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Opening_Hours_Stores", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "floorInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    modelUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floorInfo", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "interests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    group = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interests", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reach",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order = table.Column<int>(type: "int", nullable: false),
                    icon = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    borrado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reach", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order = table.Column<int>(type: "int", nullable: false),
                    icon = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shop",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<int>(type: "int", nullable: false),
                    logo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    photo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    firstOpeningDay = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shopCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopCategory", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShopInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopInfo", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shopSubCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopSubCategory", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Time_Range",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time_Range", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Floor_Nav_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nodeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    attachedShopNode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LineaShopCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_shop = table.Column<int>(type: "int", nullable: false),
                    id_category = table.Column<int>(type: "int", nullable: false),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LineaShopSubCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_shop = table.Column<int>(type: "int", nullable: false),
                    id_subcategory = table.Column<int>(type: "int", nullable: false),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "oDay",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Floor_Shop",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nodeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    attachedShopid = table.Column<int>(type: "int", nullable: false),
                    FloorInfoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor_Shop", x => x.id);
                    table.ForeignKey(
                        name: "FK_Floor_Shop_floorInfo_FloorInfoid",
                        column: x => x.FloorInfoid,
                        principalTable: "floorInfo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Floor_Shop_ShopInfo_attachedShopid",
                        column: x => x.attachedShopid,
                        principalTable: "ShopInfo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "opening_general",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    globalid = table.Column<int>(type: "int", nullable: false),
                    foodid = table.Column<int>(type: "int", nullable: false),
                    hypermarketid = table.Column<int>(type: "int", nullable: false),
                    ourStoresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opening_general", x => x.id);
                    table.ForeignKey(
                        name: "FK_opening_general_Day_Opening_Hours_Food_foodid",
                        column: x => x.foodid,
                        principalTable: "Day_Opening_Hours_Food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_general_Day_Opening_Hours_Hipermarket_hypermarketid",
                        column: x => x.hypermarketid,
                        principalTable: "Day_Opening_Hours_Hipermarket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_general_Day_Opening_Hours_Stores_ourStoresid",
                        column: x => x.ourStoresid,
                        principalTable: "Day_Opening_Hours_Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_general_Time_Range_globalid",
                        column: x => x.globalid,
                        principalTable: "Time_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LineaInterest",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_event = table.Column<int>(type: "int", nullable: false),
                    id_interest = table.Column<int>(type: "int", nullable: false),
                    id_promotion = table.Column<int>(type: "int", nullable: false),
                    Mall_Eventid = table.Column<int>(type: "int", nullable: true),
                    Promotionid = table.Column<int>(type: "int", nullable: true),
                    Shopid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaInterest", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineaInterest_events_Mall_Eventid",
                        column: x => x.Mall_Eventid,
                        principalTable: "events",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LineaInterest_promotion_Promotionid",
                        column: x => x.Promotionid,
                        principalTable: "promotion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LineaInterest_shop_Shopid",
                        column: x => x.Shopid,
                        principalTable: "shop",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "opening_hours",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    generalid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opening_hours", x => x.id);
                    table.ForeignKey(
                        name: "FK_opening_hours_opening_general_generalid",
                        column: x => x.generalid,
                        principalTable: "opening_general",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "opening_exceptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dateRangeid = table.Column<int>(type: "int", nullable: false),
                    globalid = table.Column<int>(type: "int", nullable: false),
                    foodid = table.Column<int>(type: "int", nullable: false),
                    hypermarketid = table.Column<int>(type: "int", nullable: false),
                    ourStoresid = table.Column<int>(type: "int", nullable: false),
                    Openingid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opening_exceptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_opening_exceptions_Date_Range_dateRangeid",
                        column: x => x.dateRangeid,
                        principalTable: "Date_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_exceptions_Day_Opening_Hours_Food_foodid",
                        column: x => x.foodid,
                        principalTable: "Day_Opening_Hours_Food",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_exceptions_Day_Opening_Hours_Hipermarket_hypermarket~",
                        column: x => x.hypermarketid,
                        principalTable: "Day_Opening_Hours_Hipermarket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_exceptions_Day_Opening_Hours_Stores_ourStoresid",
                        column: x => x.ourStoresid,
                        principalTable: "Day_Opening_Hours_Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opening_exceptions_opening_hours_Openingid",
                        column: x => x.Openingid,
                        principalTable: "opening_hours",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_opening_exceptions_Time_Range_globalid",
                        column: x => x.globalid,
                        principalTable: "Time_Range",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_events_dateRangeid",
                table: "events",
                column: "dateRangeid");

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
                name: "IX_LineaInterest_Mall_Eventid",
                table: "LineaInterest",
                column: "Mall_Eventid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaInterest_Promotionid",
                table: "LineaInterest",
                column: "Promotionid");

            migrationBuilder.CreateIndex(
                name: "IX_LineaInterest_Shopid",
                table: "LineaInterest",
                column: "Shopid");

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
                name: "IX_opening_exceptions_dateRangeid",
                table: "opening_exceptions",
                column: "dateRangeid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_exceptions_foodid",
                table: "opening_exceptions",
                column: "foodid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_exceptions_globalid",
                table: "opening_exceptions",
                column: "globalid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_exceptions_hypermarketid",
                table: "opening_exceptions",
                column: "hypermarketid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_exceptions_Openingid",
                table: "opening_exceptions",
                column: "Openingid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_exceptions_ourStoresid",
                table: "opening_exceptions",
                column: "ourStoresid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_general_foodid",
                table: "opening_general",
                column: "foodid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_general_globalid",
                table: "opening_general",
                column: "globalid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_general_hypermarketid",
                table: "opening_general",
                column: "hypermarketid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_general_ourStoresid",
                table: "opening_general",
                column: "ourStoresid");

            migrationBuilder.CreateIndex(
                name: "IX_opening_hours_generalid",
                table: "opening_hours",
                column: "generalid");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_dateRangeid",
                table: "promotion",
                column: "dateRangeid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "opening_exceptions");

            migrationBuilder.DropTable(
                name: "reach");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "shopCategory");

            migrationBuilder.DropTable(
                name: "shopSubCategory");

            migrationBuilder.DropTable(
                name: "floorInfo");

            migrationBuilder.DropTable(
                name: "ShopInfo");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "shop");

            migrationBuilder.DropTable(
                name: "opening_hours");

            migrationBuilder.DropTable(
                name: "Date_Range");

            migrationBuilder.DropTable(
                name: "opening_general");

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
