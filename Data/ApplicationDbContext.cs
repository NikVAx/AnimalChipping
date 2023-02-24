using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext
        : DbContext
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
                   .HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(x => x.ChipperId);
 
            builder.Entity<Animal>()
                   .HasMany(x => x.AnimalTypes)
                   .WithMany();

            builder.Entity<Animal>()
                   .HasMany(x => x.VisitedLocations)
                   .WithMany();

            base.OnModelCreating(builder);
        }
    }
}

/*
 System.InvalidOperationException:
  "Both relationships between 'AnimalVisitedLocation.LocationPoint' and
  'LocationPoint' and between 'AnimalVisitedLocation' and 'LocationPoint'
could use {'LocationPointId'} as the foreign key.

To resolve this, configure the foreign key properties explicitly in 'OnModelCreating' on at least one of the relationships."

 
 */