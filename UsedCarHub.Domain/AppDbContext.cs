using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Common;
using UsedCarHub.Domain.Configurations;

namespace UsedCarHub.Domain
{
    public sealed class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, string,
        IdentityUserClaim<string>, UserRoleEntity, IdentityUserLogin<string>, IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CarEntity> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyUtcDateTimeConverter();
        }
    }
}