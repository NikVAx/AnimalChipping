using Application.Abstractions.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext
        : DbContext, IApplicationDbContext
    {
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Animal>()
                   .HasOne(x => x.Chipper)
                   .WithMany()
                   .HasForeignKey(x => x.ChipperId);
            
           //builder.Entity<Animal>()
           //       .HasMany(x => x.AnimalTypes)
           //       .WithMany(x => x.Animals);

            builder.Entity<Animal>()
                   .HasMany(x => x.VisitedLocations)
                   .WithMany();

            builder.Entity<Account>()
                   .HasIndex(x => x.Email)
                   .IsUnique();
            
            builder.Entity<AnimalType>()
                   .HasIndex(x => x.Type)
                   .IsUnique();
            
            builder.Entity<LocationPoint>()
                   .HasIndex(x => new { x.Latitude, x.Longitude })
                   .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}