using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class SalesReceiptConfiguration : IEntityTypeConfiguration<SalesReceipt>
    {
        public void Configure(EntityTypeBuilder<SalesReceipt> entity)
        {
            entity.ToTable(nameof(SalesReceipt), "Accounting");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.SalesInvoiceId)
                .HasColumnName("SalesInvoiceId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.ReceiptNumber)
                .HasColumnName("ReceiptNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.AmountReceived)
                .HasColumnName("AmountReceived")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.PaymentMethodId)
                .HasColumnName("PaymentMethodId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.ReceiptDate)
                .HasColumnName("ReceiptDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(s => s.Reference)
                .HasColumnName("Reference")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.HasOne(s => s.SalesInvoice)
                .WithMany(i => i.Receipts)
                .HasForeignKey(s => s.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<SalesReceipt>.AuditableEntityConfigurations(entity);
        }
    }
}
