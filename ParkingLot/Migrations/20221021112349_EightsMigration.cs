using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLot.Migrations
{
    public partial class EightsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Cars");
            migrationBuilder.DropTable(name: "Buses");
            migrationBuilder.DropTable(name: "Trucks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
