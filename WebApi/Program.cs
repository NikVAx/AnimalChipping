using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using WebApi.AutoMapper;
using WebApi.Filters;
using WebApi.Handlers;
using WebApi.Middleware;
using WebApi.Requirements;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddDataLayer(configuration);
            builder.Services.AddApplicationLayer();
            builder.Services.AddAutoMapper(typeof(DtoMappingProfile));


            builder.Services.AddControllers(options => {
                options.Filters.Add<ExceptionFilterAttribute>();
                options.Filters.Add<ValidateModelFilerAttribute>();
            });

            builder.Services.AddCors();
            
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options => {
                options.DescribeAllParametersInCamelCase();
            });
 

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationPolicies.Identified, policy =>
                {
                    policy.Requirements.Add(new IdentifiedRequirement());
                });
            });

            builder.Services.AddTransient<IAuthorizationHandler, IdentifiedHandler>();

            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler,
                AppAuthorizationMiddlewareResultHandler>();

            var app = builder.Build();

            
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                DbInitializer.RecreateDatabase(services);
            }

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}