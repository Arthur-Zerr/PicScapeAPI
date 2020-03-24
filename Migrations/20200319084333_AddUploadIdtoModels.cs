using Microsoft.EntityFrameworkCore.Migrations;

namespace PicScapeAPI.Migrations
{
    public partial class AddUploadIdtoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadId",
                table: "ProfilePictures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadId",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadId",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "UploadId",
                table: "Pictures");
        }
    }
}
