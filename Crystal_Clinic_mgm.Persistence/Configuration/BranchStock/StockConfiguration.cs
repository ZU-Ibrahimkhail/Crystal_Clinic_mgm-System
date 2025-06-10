using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Stocks
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> entity)
        {
            entity.ToTable(nameof(Stock), "Stock");

            // Primary Key configuration
            entity.HasKey(s => s.stockId);

            // Property configurations
            entity.Property(s => s.quantity)
                  .HasColumnName("Quantity")
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(s => s.itemId)
                  .HasColumnName("ItemId")
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(s => s.BranchId)
                  .HasColumnName("BranchId")
                  .HasColumnType("int")
                  .IsRequired();

            // Relationship configuration
            entity.HasOne(s => s.item)
                  .WithMany() // Assuming the relationship with Item is not one-to-many
                  .HasForeignKey(s => s.itemId)
                  .OnDelete(DeleteBehavior.Restrict); // Handle as per your business needs (Restrict for now)

            entity.Property(s => s.purchasePrice)
                  .HasColumnName("PurchasePrice")
                  .HasColumnType("decimal(18, 2)")
                  .IsRequired();

            entity.Property(s => s.sellPrice)
                  .HasColumnName("SellPrice")
                  .HasColumnType("decimal(18, 2)")
                  .IsRequired();

            entity.Property(s => s.purchaseDate)
                  .HasColumnName("PurchaseDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(s => s.batchNumber)
                  .HasColumnName("BatchNumber")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(s => s.barCode)
                  .HasColumnName("BarCode")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(s => s.expiryDate)
                  .HasColumnName("ExpiryDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            // Auditable properties
            EntityConfiguration<Stock>.AuditableEntityConfigurations((EntityTypeBuilder<Stock>)entity);
        }
    }
}
