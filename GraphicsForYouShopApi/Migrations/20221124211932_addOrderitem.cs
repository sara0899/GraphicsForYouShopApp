using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphicsForYouShopApi.Migrations
{
    public partial class addOrderitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "OrderItems");
        }
    }
}
