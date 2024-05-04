using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Domain
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("UserSeed.json");
            var users = JsonSerializer.Deserialize<List<UserEntity>>(userData);
            if (users == null) return;

            var roles = new List<RoleEntity>
            {
                new RoleEntity { Name = "Admin" },
                new RoleEntity { Name = "Seller" },
                new RoleEntity { Name = "Purchaser" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "12345Chupakabra");
                await userManager.AddToRoleAsync(user, "Purchaser");
                if (user.PhoneNumber == "+11111111")
                    await userManager.AddToRoleAsync(user, "Seller");
            }

            var admin = new UserEntity()
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin10@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                PhoneNumber = "+22222222"
            };

            await userManager.CreateAsync(admin, "12345Admin");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}