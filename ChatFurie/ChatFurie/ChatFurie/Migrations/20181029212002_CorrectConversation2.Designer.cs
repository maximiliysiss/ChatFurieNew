﻿// <auto-generated />
using System;
using AuthServiceWCF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatFurie.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20181029212002_CorrectConversation2")]
    partial class CorrectConversation2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatWCF.Models.AddFriendNotification", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Context");

                    b.Property<DateTime>("DateTime");

                    b.Property<int?>("DialogID");

                    b.Property<bool>("IsDialog");

                    b.Property<bool>("IsRead");

                    b.Property<int>("UserFromID");

                    b.Property<int>("UserToID");

                    b.HasKey("ID");

                    b.HasIndex("DialogID");

                    b.HasIndex("UserFromID");

                    b.HasIndex("UserToID");

                    b.ToTable("AddFriendNotifications");
                });

            modelBuilder.Entity("ChatWCF.Models.CommonNotification", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Context");

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("IsRead");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("CommonNotifications");
                });

            modelBuilder.Entity("ChatWCF.Models.Conversation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatorID");

                    b.Property<DateTime>("DateStart");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("CreatorID");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("ChatWCF.Models.ConversationMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorID");

                    b.Property<string>("Content");

                    b.Property<int>("ConversationID");

                    b.Property<DateTime>("DateTime");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.HasIndex("ConversationID");

                    b.ToTable("ConversationMessages");
                });

            modelBuilder.Entity("ChatWCF.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Image")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("DefaultIconAccount.png");

                    b.Property<DateTime>("LastEnter");

                    b.Property<string>("Login");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatWCF.Models.UserInConversation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConversationID");

                    b.Property<DateTime>("DateStart");

                    b.Property<string>("Image")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("DefaultIconAccount.png");

                    b.Property<string>("Name");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("UserID");

                    b.ToTable("UserInConversation");
                });

            modelBuilder.Entity("ChatWCF.Models.UserReadMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConversationMessageID");

                    b.Property<bool>("IsRead");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ConversationMessageID");

                    b.HasIndex("UserID");

                    b.ToTable("UserReadMessages");
                });

            modelBuilder.Entity("ChatWCF.Models.AddFriendNotification", b =>
                {
                    b.HasOne("ChatWCF.Models.Conversation", "Conversation")
                        .WithMany("AddFriendNotifications")
                        .HasForeignKey("DialogID");

                    b.HasOne("ChatWCF.Models.User", "UserFrom")
                        .WithMany("AddFriendNotificationsFrom")
                        .HasForeignKey("UserFromID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatWCF.Models.User", "UserTo")
                        .WithMany("AddFriendNotificationsTo")
                        .HasForeignKey("UserToID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatWCF.Models.CommonNotification", b =>
                {
                    b.HasOne("ChatWCF.Models.User", "User")
                        .WithMany("CommonNotifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatWCF.Models.Conversation", b =>
                {
                    b.HasOne("ChatWCF.Models.User", "Creator")
                        .WithMany("Conversations")
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatWCF.Models.ConversationMessage", b =>
                {
                    b.HasOne("ChatWCF.Models.User", "Author")
                        .WithMany("ConversationMessages")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatWCF.Models.Conversation", "Conversation")
                        .WithMany("ConversationMessages")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatWCF.Models.UserInConversation", b =>
                {
                    b.HasOne("ChatWCF.Models.Conversation", "Conversation")
                        .WithMany("UserInConversations")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatWCF.Models.User", "User")
                        .WithMany("UserInConversations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatWCF.Models.UserReadMessage", b =>
                {
                    b.HasOne("ChatWCF.Models.ConversationMessage", "ConversationMessage")
                        .WithMany("UserReadMessages")
                        .HasForeignKey("ConversationMessageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatWCF.Models.User", "User")
                        .WithMany("UserReadMessages")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
