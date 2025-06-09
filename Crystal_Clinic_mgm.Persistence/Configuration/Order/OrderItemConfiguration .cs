using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.ToTable(nameof(OrderItem), "Sales");
            entity.HasKey(x => x.OrderItemId);

            entity.Property(x => x.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.OriginalPrice).HasColumnName("OriginalPrice").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.ActualPrice).HasColumnName("ActualPrice").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.TotalPrice).HasColumnName("TotalPrice").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.IsRental).HasColumnName("IsRental").HasColumnType("bit").IsRequired();
            entity.Property(x => x.RentalDays).HasColumnName("RentalDays").HasColumnType("int");

            // Relationships
            entity.HasOne(x => x.Order)
                  .WithMany(x => x.OrderItems)
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Item)
                  .WithMany(x => x.OrderItems)
                  .HasForeignKey(x => x.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Unit)
                  .WithMany()
                  .HasForeignKey(x => x.UnitId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
