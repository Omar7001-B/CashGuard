using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreeFriends.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bank_Account_ID",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Bank_Account_ID",
                table: "User",
                type: "TEXT",
                maxLength: 50,
                nullable: true);
        }
    }
}
