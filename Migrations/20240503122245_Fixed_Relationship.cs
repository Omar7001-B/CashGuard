using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreeFriends.Migrations
{
    public partial class Fixed_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "History",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_History_UserId",
                table: "History",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_User_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_User_UserId",
                table: "History",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_User_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_History_User_UserId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_UserId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");
        }
    }
}
