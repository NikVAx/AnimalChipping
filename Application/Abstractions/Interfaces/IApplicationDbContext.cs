using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public DatabaseFacade Database { get; }
    }
}
