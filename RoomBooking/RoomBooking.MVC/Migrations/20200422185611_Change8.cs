using Microsoft.EntityFrameworkCore.Migrations;

namespace RoomBooking.MVC.Migrations
{
    public partial class Change8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Rooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Rooms");
        }
    }
}
