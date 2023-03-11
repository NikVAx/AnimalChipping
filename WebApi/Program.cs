using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using WebApi.AutoMapper;
using WebApi.Filters;
using WebApi.Handlers;

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
            
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilterAttribute>();
            });

            

            builder.Services.AddCors();
            builder.Services.AddEndpointsApiExplorer(); // https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "basic",
                    Type = SecuritySchemeType.Http
                });
            });

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //builder.Services.AddAuthorization();


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