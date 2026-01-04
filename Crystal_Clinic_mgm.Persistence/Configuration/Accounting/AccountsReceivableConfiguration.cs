using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class AccountsReceivableConfiguration : IEntityTypeConfiguration<AccountsReceivable>
    {
        public void Configure(EntityTypeBuilder<AccountsReceivable> entity)
        {
            entity.ToTable(nameof(AccountsReceivable), "Accounting");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.InvoiceNumber)
                .HasColumnName("InvoiceNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.CustomerId)
                .HasColumnName("CustomerId")
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
                .HasDefaultValue(0);

            entity.Property(a => a.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(a => a.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(a => a.VisitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.ChartOfAccount)
                .WithMany()
                .HasForeignKey(a => a.ChartOfAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Branch)
                .WithMany()
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Visit)
                .WithMany()
                .HasForeignKey(a => a.VisitId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(a => a.Receipts)
                .WithOne(r => r.AccountsReceivable)
                .HasForeignKey(r => r.AccountsReceivableId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(a => a.InvoiceNumber).IsUnique();

            EntityConfiguration<AccountsReceivable>.AuditableEntityConfigurations(entity);
        }
    }
}
