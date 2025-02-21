using dotnetcorebackend.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcorebackend.Infrastructure.Context
{
    public class ApplicationDBContext : DbContext
    {
        // ApplicationDBContext Constructor
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Pin> Pins { get; set; }
        public DbSet<UserPin> UserPins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Adding Unique Constraint for Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Mapping UserPin → User
            modelBuilder.Entity<UserPin>()
                .HasOne(up => up.User)
                .WithMany(p => p.UserPins)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mapping UserPin → Pin
            modelBuilder.Entity<UserPin>()
                .HasOne(up => up.Pin)
                .WithMany(p => p.UserPins)
                .HasForeignKey(up => up.PinId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
