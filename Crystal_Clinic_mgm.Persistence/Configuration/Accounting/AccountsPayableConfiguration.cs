using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class AccountsPayableConfiguration : IEntityTypeConfiguration<AccountsPayable>
    {
        public void Configure(EntityTypeBuilder<AccountsPayable> entity)
        {
            entity.ToTable(nameof(AccountsPayable), "Accounting");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.InvoiceNumber)
                .HasColumnName("InvoiceNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.VendorBillId)
                .HasColumnName("VendorBillId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(a => a.CustomerId)
               .HasColumnName("CustomerId")
               .HasColumnType("int")
               .IsRequired(false);

            entity.Property(a => a.PurchaseOrderId)
                .HasColumnName("PurchaseOrderId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(a => a.VendorId)
                .HasColumnName("VendorId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(a => a.InvoiceDate)
                .HasColumnName("InvoiceDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(a => a.DueDate)
                .HasColumnName("DueDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(a => a.InvoiceAmount)
                .HasColumnName("InvoiceAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(a => a.PaidAmount)
                .HasColumnName("PaidAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(a => a.BalanceAmount)
                .HasColumnName("BalanceAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(a => a.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(APStatus.Draft);

            entity.Property(a => a.Type)
               .HasColumnName("Type")
               .HasColumnType("int")
               .IsRequired();

            entity.Property(a => a.Attachment)
                .HasColumnName("Attachemnt")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(a => a.Reference)
                .HasColumnName("Refrence")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            entity.Property(a => a.CurrencyRate)
                .HasColumnName("CurrencyRate")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(APStatus.Draft);

            entity.Property(a => a.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(a => a.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(a => a.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(a => a.VendorBill)
                .WithMany()
                .HasForeignKey(a => a.VendorBillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.PurchaseOrder)
                .WithMany()
                .HasForeignKey(a => a.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Vendor)
                .WithMany()
                .HasForeignKey(a => a.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.ChartOfAccount)
                .WithMany()
                .HasForeignKey(a => a.ChartOfAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Branch)
                .WithMany()
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(a => a.Payments)
                .WithOne(p => p.AccountsPayable)
                .HasForeignKey(p => p.AccountsPayableId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(a => a.InvoiceNumber).IsUnique();

            EntityConfiguration<AccountsPayable>.AuditableEntityConfigurations(entity);
        }
    }
}
