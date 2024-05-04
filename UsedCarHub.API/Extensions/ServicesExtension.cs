using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UsedCarHub.BusinessLogic.AutoMapperConfiguration;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.BusinessLogic.Services;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository;
using UsedCarHub.Repository.Interfaces;
using UsedCarHub.Repository.Repositories;

namespace UsedCarHub.API.Extensions
{
    static public class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AppMappingProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<UserEntity>(opt => { opt.Password.RequireNonAlphanumeric = false; })
                .AddRoles<RoleEntity>().AddRoleManager<RoleManager<RoleEntity>>()
                .AddSignInManager<SignInManager<UserEntity>>().AddRoleValidator<RoleValidator<RoleEntity>>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("RequireSellerRole", policy => policy.RequireRole("Seller"));
                opt.AddPolicy("RequirePurchaserRole", policy => policy.RequireRole("Purchaser"));
            });

            return services;
        }
    }
}