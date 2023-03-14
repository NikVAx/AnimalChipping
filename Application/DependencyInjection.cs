using Application.Abstractions.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services)
        {
            services.AddScoped<IAnimalTypeService, AnimalTypeService>();
            services.AddScoped<ILocationPointService, LocationPointService>();
            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAnimalLocationPointService, AnimalLocationPointService>();
            services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            return services;
        }
    }
}
