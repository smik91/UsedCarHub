using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Domain.Configurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<AdvertisementEntity>
    {
        public void Configure(EntityTypeBuilder<AdvertisementEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();
            
            builder.Property(a => a.Description).IsRequired();
            builder.Property(a => a.Price).IsRequired();

            builder.HasOne(a => a.Seller)
                .WithMany(u => u.Advertisements)
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(a => a.Car)
                .WithOne(c => c.Advertisement)
                .HasForeignKey<CarEntity>(c => c.AdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}