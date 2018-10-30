using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatFurie.Migrations
{
    public partial class CorrectNotif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "CommonNotifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "AddFriendNotifications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "CommonNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "AddFriendNotifications");
        }
    }
}
