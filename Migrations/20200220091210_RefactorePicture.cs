using Microsoft.EntityFrameworkCore.Migrations;

namespace PicScapeAPI.Migrations
{
    public partial class RefactorePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Pictures",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures",
                newName: "IX_Pictures_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_UserID",
                table: "Pictures",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UserID",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Pictures",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_UserID",
                table: "Pictures",
                newName: "IX_Pictures_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId",
                table: "Pictures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
