using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatFurie.Migrations
{
    public partial class InitConversMSGAndReadable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationMessages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationID = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    AuthorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_Users_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserReadMessages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationMessageID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReadMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserReadMessages_ConversationMessages_ConversationMessageID",
                        column: x => x.ConversationMessageID,
                        principalTable: "ConversationMessages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReadMessages_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_AuthorID",
                table: "ConversationMessages",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ConversationID",
                table: "ConversationMessages",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadMessages_ConversationMessageID",
                table: "UserReadMessages",
                column: "ConversationMessageID");

            migrationBuilder.CreateIndex(
                name: "IX_UserReadMessages_UserID",
                table: "UserReadMessages",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReadMessages");

            migrationBuilder.DropTable(
                name: "ConversationMessages");
        }
    }
}
