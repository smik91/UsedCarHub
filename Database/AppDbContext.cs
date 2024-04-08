using CarAPI.Database.Configurations;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Database
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
