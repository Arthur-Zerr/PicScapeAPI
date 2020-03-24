using Microsoft.EntityFrameworkCore.Migrations;

namespace PicScapeAPI.Migrations
{
    public partial class ChangedPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProfilePictures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProfilePictures",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
