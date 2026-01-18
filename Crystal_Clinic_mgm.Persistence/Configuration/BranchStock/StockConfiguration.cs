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

            entity.Property(s => s.SupplierId)
                  .HasColumnName("SupplierId")
                  .HasColumnType("int")
                  .IsRequired(false);

            // Relationship configuration
            entity.HasOne(s => s.item)
                  .WithMany() // Assuming the relationship with Item is not one-to-many
                  .HasForeignKey(s => s.itemId)
                  .OnDelete(DeleteBehavior.Restrict); // Handle as per your business needs (Restrict for now)

            entity.HasOne(s => s.Supplier)
                  .WithMany() 
                  .HasForeignKey(s => s.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull); 

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

            entity.Property(s => s.LotNumber)
                  .HasColumnName("LotNumber")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(s => s.ManufactureDate)
                  .HasColumnName("ManufactureDate")
                  .HasColumnType("datetime2")
                  .IsRequired(false);

            entity.Property(s => s.IsExpired)
                  .HasColumnName("IsExpired")
                  .HasColumnType("bit")
                  .IsRequired()
                  .HasDefaultValue(false);

            entity.Property(s => s.QuantityRemaining)
                  .HasColumnName("QuantityRemaining")
                  .HasColumnType("decimal(18,4)")
                  .IsRequired();

            entity.Property(s => s.SiteId)
                  .HasColumnName("SiteId")
                  .HasColumnType("int")
                  .IsRequired(false);

            entity.Property(s => s.PurchaseOrderId)
                  .HasColumnName("PurchaseOrderId")
                  .HasColumnType("int")
                  .IsRequired(false);

            entity.Property(s => s.InvoiceId)
                  .HasColumnName("InvoiceId")
                  .HasColumnType("int")
                  .IsRequired(false);

            entity.Property(s => s.FreightCost)
                  .HasColumnName("FreightCost")
                  .HasColumnType("decimal(18,4)")
                  .IsRequired()
                  .HasDefaultValue(0m);

            entity.Property(s => s.InsuranceCost)
                  .HasColumnName("InsuranceCost")
                  .HasColumnType("decimal(18,4)")
                  .IsRequired()
                  .HasDefaultValue(0m);

            entity.Property(s => s.ImportDuty)
                  .HasColumnName("ImportDuty")
                  .HasColumnType("decimal(18,4)")
                  .IsRequired()
                  .HasDefaultValue(0m);

            entity.Property(s => s.OtherLandingCosts)
                  .HasColumnName("OtherLandingCosts")
                  .HasColumnType("decimal(18,4)")
                  .IsRequired()
                  .HasDefaultValue(0m);

            // Auditable properties
            EntityConfiguration<Stock>.AuditableEntityConfigurations((EntityTypeBuilder<Stock>)entity);
        }
    }
}
