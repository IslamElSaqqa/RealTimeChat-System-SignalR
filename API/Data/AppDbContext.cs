using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AdvancedChat.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms => Set<Room>();

        public DbSet<RoomUser> RoomUsers => Set<RoomUser>();

        public DbSet<Message> Messages => Set<Message>();

        public DbSet<EventLog> EventLogs => Set<EventLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite Key
            builder.Entity<RoomUser>()
                .HasKey(x => new { x.RoomId, x.UserId });

            builder.Entity<RoomUser>()
                .HasOne(x => x.Room)
                .WithMany(x => x.RoomUsers)
                .HasForeignKey(x => x.RoomId);

            builder.Entity<RoomUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.RoomUsers)
                .HasForeignKey(x => x.UserId);

            // Prevent multiple cascade paths
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Room>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}