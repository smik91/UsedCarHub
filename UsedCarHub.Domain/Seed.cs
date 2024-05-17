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

            var userData = await File.ReadAllTextAsync("../UsedCarHub.Domain/UserSeed.json");
            var users = JsonSerializer.Deserialize<List<UserEntity>>(userData);
            if (users == null) return;

            var roles = new List<RoleEntity>
            {
                new RoleEntity { Name = "Purchaser" },
                new RoleEntity { Name = "Seller" },
                new RoleEntity { Name = "Admin" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "12345Hello");
                await userManager.AddToRoleAsync(user, "Purchaser");
                if (user.PhoneNumber == "+123456789")
                    await userManager.AddToRoleAsync(user, "Seller");
            }

            var admin = new UserEntity
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin10@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "+123456789",
                TwoFactorEnabled = false
            };
            await userManager.CreateAsync(admin, "Admin100");
            await userManager.AddToRolesAsync(admin, new[] { "Purchaser", "Seller", "Admin" });
        }
    }
}