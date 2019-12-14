using Microsoft.EntityFrameworkCore.Migrations;

namespace PicScapeAPI.Migrations
{
    public partial class PictureAddTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Pictures");
        }
    }
}
