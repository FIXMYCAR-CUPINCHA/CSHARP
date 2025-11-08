using Microsoft.EntityFrameworkCore;
using SentinelTrack.Domain.Entities;

namespace SentinelTrack.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Yard> Yards => Set<Yard>();
        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Yard>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.Property(x => x.Address).HasMaxLength(200);
                b.Property(x => x.PhoneNumber).HasMaxLength(30);
                b.Property(x => x.Capacity).IsRequired();

                b.HasMany(x => x.Motos)
                 .WithOne(x => x.Yard)
                 .HasForeignKey(x => x.YardId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Moto>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Plate).IsRequired().HasMaxLength(10);
                b.Property(x => x.Model).IsRequired().HasMaxLength(80);
                b.Property(x => x.Color).HasMaxLength(40);
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.Property(x => x.Email).IsRequired().HasMaxLength(160);
                b.Property(x => x.Role).IsRequired().HasMaxLength(40);
            });
        }
    }
}
