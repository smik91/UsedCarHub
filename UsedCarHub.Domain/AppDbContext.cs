using UsedCarHub.Domain.Entities;
using UsedCarHub.Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace UsedCarHub.Domain
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}