using ChatWCF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWCF.Models
{
    /// <summary>
    /// Main Context For ChatModels
    /// </summary>
    public class ChatContextWCF : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<UserInConversation> UserInConversation { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<UserReadMessage> UserReadMessages { get; set; }
        public DbSet<CommonNotification> CommonNotifications { get; set; }
        public DbSet<AddFriendNotification> AddFriendNotifications { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-KFM5UPB;Initial Catalog=ChatFurieNew;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(x => x.ToTable("Users"));
            modelBuilder.Entity<User>().Property(x => x.Image)
                .HasDefaultValue("DefaultIconAccount.png");

            modelBuilder.Entity<UserInConversation>().Property(x=>x.Image)
                .HasDefaultValue("DefaultIconAccount.png");

            #region UserInConversation
            modelBuilder.Entity<UserInConversation>()
                .HasOne<Conversation>(x => x.Conversation)
                .WithMany(x => x.UserInConversations)
                .HasForeignKey(x => x.ConversationID);

            modelBuilder.Entity<UserInConversation>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.UserInConversations)
                .HasForeignKey(x => x.UserID);

            #endregion UserInConversation


            #region AddFriendNotif
            modelBuilder.Entity<AddFriendNotification>()
                .HasOne<User>(x => x.UserFrom)
                .WithMany(x => x.AddFriendNotificationsFrom)
                .HasForeignKey(x => x.UserFromID);

            modelBuilder.Entity<AddFriendNotification>()
                .HasOne<User>(x => x.UserTo)
                .WithMany(x => x.AddFriendNotificationsTo)
                .HasForeignKey(x => x.UserToID);

            modelBuilder.Entity<AddFriendNotification>()
                .HasOne<Conversation>(x => x.Conversation)
                .WithMany(x => x.AddFriendNotifications)
                .HasForeignKey(x => x.DialogID)
                .IsRequired(false);

            #endregion AddFriendNotif

            modelBuilder.Entity<CommonNotification>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.CommonNotifications)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<UserReadMessage>()
                .HasOne<ConversationMessage>(x => x.ConversationMessage)
                .WithMany(x => x.UserReadMessages)
                .HasForeignKey(x => x.ConversationMessageID);

            modelBuilder.Entity<UserReadMessage>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.UserReadMessages)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<ConversationMessage>()
                .HasOne<User>(x => x.Author)
                .WithMany(x => x.ConversationMessages)
                .HasForeignKey(x => x.AuthorID);

            modelBuilder.Entity<ConversationMessage>()
                .HasOne<Conversation>(x => x.Conversation)
                .WithMany(x => x.ConversationMessages)
                .HasForeignKey(x => x.ConversationID);

            modelBuilder.Entity<Conversation>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.CreatorID);
        }
    }
}
