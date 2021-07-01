using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBlogApp.Data.Migrations
{
    public partial class categoryTableFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AuthorId",
                table: "Categories",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_AuthorId",
                table: "Categories",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_AuthorId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_AuthorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Categories");
        }
    }
}
