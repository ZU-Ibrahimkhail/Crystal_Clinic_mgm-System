using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.ToTable(nameof(PurchaseOrder), "Accounting");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.PONumber)
                .HasColumnName("PONumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.VendorId)
                .HasColumnName("VendorId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.OrderDate)
                .HasColumnName("OrderDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(p => p.ExpectedDeliveryDate)
                .HasColumnName("ExpectedDeliveryDate")
                .HasColumnType("datetime")
                .IsRequired(false);

            entity.Property(p => p.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(p => p.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(POStatus.Open);

            entity.Property(p => p.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(p => p.Vendor)
                .WithMany()
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Branch)
                .WithMany()
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(a => a.Attachment)
                .HasColumnName("Attachemnt")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.HasMany(p => p.Lines)
                .WithOne(l => l.PurchaseOrder)
                .HasForeignKey(l => l.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(p => p.PONumber).IsUnique();

            EntityConfiguration<PurchaseOrder>.AuditableEntityConfigurations(entity);
        }
    }
}
