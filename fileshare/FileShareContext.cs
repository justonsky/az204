using FileShare.Models;
using Microsoft.EntityFrameworkCore;

namespace FileShare
{
    public class FileShareContext : DbContext
    {
        public FileShareContext(
            DbContextOptions<FileShareContext> options): base(options)
        {
        }
        
        public DbSet<Room> Rooms { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.Name)
                .IsUnique();
            modelBuilder.Entity<File>()
                .HasIndex(f => new { f.RoomId, f.Name })
                .IsUnique();
        }
    }
}