using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Domain.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId).IsRequired();
            
            builder.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<ProfileEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}