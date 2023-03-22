using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class DbInitializer
    {
        public static void RecreateDatabase(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider
                .GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
