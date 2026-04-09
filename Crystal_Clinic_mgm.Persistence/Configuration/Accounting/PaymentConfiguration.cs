using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> entity)
        {
            entity.ToTable(nameof(Payment), "Accounting");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.AccountsPayableId)
                .HasColumnName("AccountsPayableId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.VendorBillId)
               .HasColumnName("VendorBillId")
               .HasColumnType("int")
               .IsRequired(false);

            entity.Property(p => p.PaymentNumber)
                .HasColumnName("PaymentNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.PaymentDate)
                .HasColumnName("PaymentDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(p => p.AmountPaid)
                .HasColumnName("AmountPaid")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(p => p.PaymentMethodId)
                .HasColumnName("PaymentMethodId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.Reference)
                .HasColumnName("Reference")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(p => p.CurrencyId)
                .HasColumnName("CurrencyId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(p => p.ExchangeRate)
                .HasColumnName("ExchangeRate")
                .HasColumnType("decimal(18, 6)")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(p => p.AmountInBaseCurrency)
                .HasColumnName("AmountInBaseCurrency")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.HasOne(p => p.AccountsPayable)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.AccountsPayableId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.VendorBill)
                .WithMany(vb => vb.Payments)
                .HasForeignKey(p => p.VendorBillId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Currency)
                .WithMany()
                .HasForeignKey(p => p.CurrencyId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<Payment>.AuditableEntityConfigurations(entity);
        }
    }
}
