using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Domain.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<CarEntity>
    {
        public void Configure(EntityTypeBuilder<CarEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.RegistrationNumber).IsRequired();
            builder.Property(c => c.Mark).IsRequired();
            builder.Property(c => c.Model).IsRequired();

            builder.HasOne(c => c.Owner)
                .WithMany(u => u.CarsForSale)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}