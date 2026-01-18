using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> entity)
        {
            entity.ToTable(nameof(StockMovement), "Stock");
            entity.HasKey(x => x.StockMovementId);

            entity.Property(x => x.Date).HasColumnName("Date").HasColumnType("datetime2").IsRequired();
            entity.Property(x => x.MovementType).HasColumnName("MovementType").HasColumnType("nvarchar(10)").IsRequired();
            entity.Property(x => x.Quantity).HasColumnName("Quantity").HasPrecision(18, 4).IsRequired();
            entity.Property(x => x.Notes).HasColumnName("Notes").HasColumnType("nvarchar(500)");
            
            entity.Property(x => x.UnitCost).HasColumnName("UnitCost").HasPrecision(18, 4).IsRequired();
            entity.Property(x => x.TotalCost).HasColumnName("TotalCost").HasPrecision(18, 4).IsRequired();
            entity.Property(x => x.Reason).HasColumnName("Reason").HasColumnType("int").IsRequired();
            entity.Property(x => x.SourceBranchId).HasColumnName("SourceBranchId").HasColumnType("int").IsRequired(false);
            entity.Property(x => x.ProcessedBy).HasColumnName("ProcessedBy").HasColumnType("UNIQUEIDENTIFIER").IsRequired(false);
            entity.Property(x => x.StockId).HasColumnName("StockId").HasColumnType("int").IsRequired(false);
            entity.Property(x => x.AdjustmentCategoryId).HasColumnName("AdjustmentCategoryId").HasColumnType("int").IsRequired(false);
            entity.Property(x => x.ReferenceId).HasColumnName("ReferenceId").HasColumnType("nvarchar(100)");

            // Relationships
            entity.HasOne(x => x.Item)
                  .WithMany()
                  .HasForeignKey(x => x.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
