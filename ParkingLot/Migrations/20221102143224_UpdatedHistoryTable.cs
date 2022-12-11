using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLot.Migrations
{
    public partial class UpdatedHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverFullName",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverFullName",
                table: "History");
        }
    }
}
