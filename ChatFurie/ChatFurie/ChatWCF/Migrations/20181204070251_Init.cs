using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatWCF.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    LastEnter = table.Column<DateTime>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true, defaultValue: "DefaultIconAccount.png"),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CommonNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Context = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CommonNotifications_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DateStart = table.Column<DateTime>(nullable: false),
                    CreatorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Conversation_Users_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddFriendNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Context = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    UserFromID = table.Column<int>(nullable: false),
                    UserToID = table.Column<int>(nullable: false),
                    IsDialog = table.Column<bool>(nullable: false),
                    DialogID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddFriendNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddFriendNotifications_Conversation_DialogID",
                        column: x => x.DialogID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddFriendNotifications_Users_UserFromID",
                        column: x => x.UserFromID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddFriendNotifications_Users_UserToID",
                        column: x => x.UserToID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMessages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationID = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DateTime = table.Column<string>(nullable: true),
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
                name: "UserInConversation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    ConversationID = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true, defaultValue: "DefaultIconAccount.png"),
                    DateStart = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInConversation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserInConversation_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInConversation_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
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
                name: "IX_AddFriendNotifications_DialogID",
                table: "AddFriendNotifications",
                column: "DialogID");

            migrationBuilder.CreateIndex(
                name: "IX_AddFriendNotifications_UserFromID",
                table: "AddFriendNotifications",
                column: "UserFromID");

            migrationBuilder.CreateIndex(
                name: "IX_AddFriendNotifications_UserToID",
                table: "AddFriendNotifications",
                column: "UserToID");

            migrationBuilder.CreateIndex(
                name: "IX_CommonNotifications_UserID",
                table: "CommonNotifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_CreatorID",
                table: "Conversation",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_AuthorID",
                table: "ConversationMessages",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ConversationID",
                table: "ConversationMessages",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserInConversation_ConversationID",
                table: "UserInConversation",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserInConversation_UserID",
                table: "UserInConversation",
                column: "UserID");

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
                name: "AddFriendNotifications");

            migrationBuilder.DropTable(
                name: "CommonNotifications");

            migrationBuilder.DropTable(
                name: "UserInConversation");

            migrationBuilder.DropTable(
                name: "UserReadMessages");

            migrationBuilder.DropTable(
                name: "ConversationMessages");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
