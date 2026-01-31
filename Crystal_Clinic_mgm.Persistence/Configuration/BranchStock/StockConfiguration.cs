using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
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
            entity.HasKey(s => s.StockId);

            // Property configurations
            entity.Property(s => s.Quantity)
                  .HasColumnName("Quantity")
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(s => s.ItemId)
                  .HasColumnName("ItemId")
                  .HasColumnType("int")
                  .IsRequired(false);

            entity.Property(s => s.BranchId)
                  .HasColumnName("BranchId")
                  .IsRequired(false);

            entity.HasOne(i => i.Branch)
                    .WithMany()
                    .HasForeignKey(i => i.BranchId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.Property(s => s.SupplierId)
                  .HasColumnName("SupplierId")
                  .HasColumnType("int")
                  .IsRequired(false);

            // Relationship configuration
            entity.HasOne(s => s.Item)
                  .WithMany() // Assuming the relationship with Item is not one-to-many
                  .HasForeignKey(s => s.ItemId)
                  .OnDelete(DeleteBehavior.Restrict); // Handle as per your business needs (Restrict for now)

            entity.HasOne(s => s.Supplier)
                  .WithMany() 
                  .HasForeignKey(s => s.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull); 

            entity.Property(s => s.PurchasePrice)
                  .HasColumnName("PurchasePrice")
                  .HasColumnType("decimal(18, 2)")
                  .IsRequired();

            entity.Property(s => s.SellPrice)
                  .HasColumnName("SellPrice")
                  .HasColumnType("decimal(18, 2)")
                  .IsRequired();

            entity.Property(s => s.PurchaseDate)
                  .HasColumnName("PurchaseDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(s => s.BatchNumber)
                  .HasColumnName("BatchNumber")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(s => s.BarCode)
                  .HasColumnName("BarCode")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(s => s.ExpiryDate)
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

            entity.HasOne(i => i.PurchaseOrder)
                .WithMany()
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.NoAction);

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
