using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(
        this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration
                .GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            //services.AddScoped<IApplicationDbContext>(provider =>
            //    provider.GetService<ApplicationDbContext>());
            services.AddScoped<ApplicationDbContext>();

            return services;
        }
    }
}
