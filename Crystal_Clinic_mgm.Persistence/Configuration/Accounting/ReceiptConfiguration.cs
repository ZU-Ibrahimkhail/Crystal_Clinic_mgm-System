using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> entity)
        {
            entity.ToTable(nameof(Receipt), "Accounting");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.AccountsReceivableId)
                .HasColumnName("AccountsReceivableId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(r => r.ReceiptNumber)
                .HasColumnName("ReceiptNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.ReceiptDate)
                .HasColumnName("ReceiptDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(r => r.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(r => r.PaymentMethodId)
                .HasColumnName("PaymentMethodId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(r => r.Reference)
                .HasColumnName("Reference")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(r => r.CurrencyId)
                .HasColumnName("CurrencyId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(r => r.ExchangeRate)
                .HasColumnName("ExchangeRate")
                .HasColumnType("decimal(18, 6)")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(r => r.AmountInBaseCurrency)
                .HasColumnName("AmountInBaseCurrency")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.HasOne(r => r.AccountsReceivable)
                .WithMany(a => a.Receipts)
                .HasForeignKey(r => r.AccountsReceivableId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Currency)
                .WithMany()
                .HasForeignKey(r => r.CurrencyId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<Receipt>.AuditableEntityConfigurations(entity);
        }
    }
}
