using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order.Look
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> entity)
        {
            entity.ToTable(nameof(Item), "Inventory");
            entity.HasKey(x => x.ItemId);

            entity.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.IsSellable).HasColumnName("IsSellable").HasColumnType("bit").HasDefaultValue(false);
            entity.Property(x => x.IsRentable).HasColumnName("IsRentable").HasColumnType("bit").HasDefaultValue(false);
            entity.Property(x => x.BaseUnit).HasColumnName("BaseUnit").HasColumnType("nvarchar(20)").IsRequired();
            entity.Property(x => x.CurrentStock).HasColumnName("CurrentStock").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.RealTimeAvailableStock).HasColumnName("RealTimeAvailableStock").HasColumnType("decimal(18,2)").IsRequired().HasDefaultValue(0);
            entity.Property(x => x.CleaningStateQuantity).HasColumnName("CleaningStateQuantity").HasColumnType("decimal(18,2)").IsRequired().HasDefaultValue(0);

            // Relationship with Category
            entity.HasOne(x => x.Category)
                  .WithMany(x => x.Items)
                  .HasForeignKey(x => x.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
