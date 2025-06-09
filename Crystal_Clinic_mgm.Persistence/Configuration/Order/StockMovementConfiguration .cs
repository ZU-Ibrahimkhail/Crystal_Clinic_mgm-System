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
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> entity)
        {
            entity.ToTable(nameof(StockMovement), "Inventory");
            entity.HasKey(x => x.StockMovementId);

            entity.Property(x => x.Date).HasColumnName("Date").HasColumnType("datetime2").IsRequired();
            entity.Property(x => x.MovementType).HasColumnName("MovementType").HasColumnType("nvarchar(10)").IsRequired();
            entity.Property(x => x.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Notes).HasColumnName("Notes").HasColumnType("nvarchar(500)");

            // Relationships
            entity.HasOne(x => x.Item)
                  .WithMany()
                  .HasForeignKey(x => x.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Order)
                  .WithMany()
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
