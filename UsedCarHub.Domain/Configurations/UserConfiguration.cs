using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Domain.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName).IsRequired(false);
            builder.Property(u =>u.LastName).IsRequired(false);

            builder.Property(u => u.UserName).IsRequired(true);
            builder.Property(u => u.Email).IsRequired(true);
            builder.Property(x => x.PasswordHash).IsRequired(true);
        }
    }
}
