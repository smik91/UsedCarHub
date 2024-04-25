using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.API.Extensions;
using UsedCarHub.BusinessLogic.AutoMapperConfiguration;
using UsedCarHub.Domain;

namespace UsedCarHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCustomServices();
            builder.Services.AddAuthenticationServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));
            
            var connection = builder.Configuration.GetConnectionString(name: "DefaultConnectionString");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

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

            app.Run();
        }
    }
}
