using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using UsedCarHub.API.Extensions;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
            
            app.UseHttpsRedirection();

            app.UseCookieProvider();
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();
            
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var db = services.GetRequiredService<AppDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<RoleEntity>>();
            var userManager = services.GetRequiredService<UserManager<UserEntity>>();
            await Seed.SeedUsers(userManager, roleManager, db);
            
            app.Run();
        }
    }
}
