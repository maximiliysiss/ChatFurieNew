using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatFurie.Migrations
{
    public partial class CorrectFriendNotif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddFriendNotifications_Conversation_DialogID",
                table: "AddFriendNotifications");

            migrationBuilder.AlterColumn<int>(
                name: "DialogID",
                table: "AddFriendNotifications",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AddFriendNotifications_Conversation_DialogID",
                table: "AddFriendNotifications",
                column: "DialogID",
                principalTable: "Conversation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddFriendNotifications_Conversation_DialogID",
                table: "AddFriendNotifications");

            migrationBuilder.AlterColumn<int>(
                name: "DialogID",
                table: "AddFriendNotifications",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddFriendNotifications_Conversation_DialogID",
                table: "AddFriendNotifications",
                column: "DialogID",
                principalTable: "Conversation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
