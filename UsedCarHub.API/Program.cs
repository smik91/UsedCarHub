using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UsedCarHub.BusinessLogic.AutoMapperConfiguration;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.BusinessLogic.Services;
using UsedCarHub.Common.Auth;
using UsedCarHub.Common.Interfaces;
using UsedCarHub.Domain;
using UsedCarHub.Repository.Interfaces;
using UsedCarHub.Repository.Repositories;

namespace UsedCarHub.API
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
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            

            var connection = builder.Configuration.GetConnectionString(name: "PostgreSqlConnectionString");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

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
            
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["usedCarHubId"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Append("Authorization", "Bearer " + token);
                    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Append("X-Xss-Protection", "1");
                    context.Response.Headers.Append("X-Frame-Options", "DENY");
                }
                await next();
            });
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
