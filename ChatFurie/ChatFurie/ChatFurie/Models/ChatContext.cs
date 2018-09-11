﻿using ChatFurie.Models.ChatModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Models
{
    /// <summary>
    /// Main Context For ChatModels
    /// </summary>
    public class ChatContext : DbContext
    {

        public DbSet<User> Users { get; set; }


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
                .HasDefaultValue("/DefaultIconAccount.png");
        }
    }
}