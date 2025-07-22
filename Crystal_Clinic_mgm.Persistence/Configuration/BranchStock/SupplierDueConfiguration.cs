using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class SupplierDueConfiguration : IEntityTypeConfiguration<SupplierDue>
    {
        public void Configure(EntityTypeBuilder<SupplierDue> entity)
        {
            // Table Name
            entity.ToTable(nameof(SupplierDue), "BranchStock");

            // Primary Key
            entity.HasKey(sd => sd.Id);

            // Foreign Key relationships
            entity.HasOne(sd => sd.Supplier)
                .WithMany()
                .HasForeignKey(sd => sd.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Supplier if SupplierDue exists

            //entity.HasOne(sd => sd.Stock)
            //    .WithMany()
            //    .HasForeignKey(sd => sd.StockId)
            //    .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Stock if SupplierDue exists

            entity.HasOne(sd => sd.CurrencyType)
                .WithMany()
                .HasForeignKey(sd => sd.CurrencyTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting CurrencyType if SupplierDue exists

            // Collection Properties
            entity.HasMany(sd => sd.Payments)
                .WithOne()
                .HasForeignKey(dp => dp.SupplierDueId)
                .OnDelete(DeleteBehavior.Cascade); // Delete DuePayments if SupplierDue is deleted

            // Properties configurations
            entity.Property(sd => sd.SupplierId)
                .HasColumnName("SupplierId")
                .IsRequired();

            //entity.Property(sd => sd.ItemId)
            //    .HasColumnName("ItemId")
            //    .IsRequired();

            //entity.Property(sd => sd.StockId)
            //    .HasColumnName("StockId")
            //    .IsRequired();

            entity.Property(sd => sd.DueAmount)
                .HasColumnName("DueAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(sd => sd.PaidAmount)
                .HasColumnName("PaidAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(sd => sd.RemainAmount)
                .HasColumnName("RemainAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(sd => sd.CurrencyTypeId)
                .HasColumnName("CurrencyTypeId")
                .IsRequired();

            // Auditable entity fields configuration
            EntityConfiguration<SupplierDue>.AuditableEntityConfigurations(entity);
        }
    }
}