using Microsoft.EntityFrameworkCore;
using UsedCarHub.Domain;
using UsedCarHub.Repository.Interfaces;
using UsedCarHub.Repository.Repositories;
namespace UsedCarHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ICarRepository,CarRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var connection = builder.Configuration.GetConnectionString(name: "DefaultConnectionString");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
