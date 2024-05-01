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

            var userData = await File.ReadAllTextAsync("../Domain/UserSeed.json");
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
                await userManager.CreateAsync(user, "12345");
                await userManager.AddToRoleAsync(user, "Seller");
                if (user.PhoneNumber == "+11111111")
                    await userManager.AddToRoleAsync(user, "Purchaser");
            }

            var admin = new UserEntity()
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false
            };

            await userManager.CreateAsync(admin, "123");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Seller", "Purchaser" });
        }
    }
}