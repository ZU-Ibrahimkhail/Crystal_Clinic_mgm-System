using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class VendorBillConfiguration : IEntityTypeConfiguration<VendorBill>
    {
        public void Configure(EntityTypeBuilder<VendorBill> entity)
        {
            entity.ToTable(nameof(VendorBill), "Accounting");

            entity.HasKey(v => v.Id);

            entity.Property(v => v.BillNumber)
                .HasColumnName("BillNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.PurchaseOrderId)
                .HasColumnName("PurchaseOrderId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(v => v.VendorId)
                .HasColumnName("VendorId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(v => v.BillDate)
                .HasColumnName("BillDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(v => v.DueDate)
                .HasColumnName("DueDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(v => v.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(v => v.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(BillStatus.Unpaid);

            entity.Property(v => v.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(v => v.PurchaseOrder)
                .WithMany()
                .HasForeignKey(v => v.PurchaseOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(v => v.Vendor)
                .WithMany()
                .HasForeignKey(v => v.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(v => v.Branch)
                .WithMany()
                .HasForeignKey(v => v.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(v => v.BillNumber).IsUnique();

            EntityConfiguration<VendorBill>.AuditableEntityConfigurations(entity);
        }
    }
}
