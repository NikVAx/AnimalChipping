using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper;

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
            builder.Services.AddControllers();
            builder.Services.AddCors(); 
            builder.Services.AddEndpointsApiExplorer(); // https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                DbInitializer.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}