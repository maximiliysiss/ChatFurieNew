using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatFurie.Migrations
{
    public partial class CorrectConversation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Users",
                nullable: true,
                defaultValue: "DefaultIconAccount.png",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "/DefaultIconAccount.png");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "UserInConversation",
                nullable: true,
                defaultValue: "DefaultIconAccount.png",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "/DefaultIconAccount.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Users",
                nullable: true,
                defaultValue: "/DefaultIconAccount.png",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "DefaultIconAccount.png");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "UserInConversation",
                nullable: true,
                defaultValue: "/DefaultIconAccount.png",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "DefaultIconAccount.png");
        }
    }
}
