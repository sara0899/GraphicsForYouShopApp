using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphicsForYouShopApi.Migrations
{
    public partial class ChangeCOlumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "GraphicPictures",
                newName: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "GraphicPictures",
                newName: "URL");
        }
    }
}
