﻿// <auto-generated />
using System;
using ChatFurie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatFurie.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20180913061339_InitMigrationUinC")]
    partial class InitMigrationUinC
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatFurie.Models.ChatModel.Conversation", b =>
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

            modelBuilder.Entity("ChatFurie.Models.ChatModel.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Image")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("/DefaultIconAccount.png");

                    b.Property<DateTime>("LastEnter");

                    b.Property<string>("Login");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatFurie.Models.ChatModel.UserInConversation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConversationID");

                    b.Property<DateTime>("DateStart");

                    b.Property<string>("Image");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("UserID");

                    b.ToTable("UserInConversation");
                });

            modelBuilder.Entity("ChatFurie.Models.ChatModel.Conversation", b =>
                {
                    b.HasOne("ChatFurie.Models.ChatModel.User", "Creator")
                        .WithMany("Conversations")
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChatFurie.Models.ChatModel.UserInConversation", b =>
                {
                    b.HasOne("ChatFurie.Models.ChatModel.Conversation", "Conversation")
                        .WithMany("UserInConversations")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatFurie.Models.ChatModel.User", "User")
                        .WithMany("UserInConversations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
