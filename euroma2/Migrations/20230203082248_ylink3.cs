using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace euroma2.Migrations
{
    public partial class ylink3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
               name: "youtubeLink",
               table: "events",
               nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
