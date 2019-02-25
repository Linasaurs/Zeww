using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class ZewwDbContext : DbContext {

        public ZewwDbContext(DbContextOptions<ZewwDbContext> options)
            : base(options) { }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserWorkspace>()
                .HasKey(uw => new { uw.WorkspaceId, uw.UserId });

            modelBuilder.Entity<UserChats>()
                .HasKey(uw => new { uw.ChatId, uw.UserId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connection = @"Server=.\sqlexpress;Database=ZewwDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(connection);
        }

    }
}