using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class DuePaymentConfiguration : IEntityTypeConfiguration<DuePayment>
    {
        public void Configure(EntityTypeBuilder<DuePayment> entity)
        {
            // Table Name
            entity.ToTable(nameof(DuePayment), "BranchStock");

            // Primary Key
            entity.HasKey(dp => dp.DuePaymentId);

            
            entity.HasOne(dp => dp.CurrencyType)
                .WithMany()
                .HasForeignKey(dp => dp.CurrencyTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting CurrencyType if DuePayment exists

            // Properties configurations
            entity.Property(dp => dp.SupplierDueId)
                .HasColumnName("SupplierDueId")
                .IsRequired();

            entity.Property(dp => dp.CurrencyTypeId)
                .HasColumnName("CurrencyTypeId");

            entity.Property(dp => dp.ExchangeRateToDueCurrency)
                .HasColumnName("ExchangeRateToDueCurrency")
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            entity.Property(dp => dp.AmmountPaid)
                .HasColumnName("AmmountPaid")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(dp => dp.AmountInDueCurrency)
                .HasColumnName("AmountInDueCurrency")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(dp => dp.paymentDate)
                .HasColumnName("PaymentDate")
                .HasColumnType("datetime")
                .IsRequired();

            // Auditable entity fields configuration
            EntityConfiguration<DuePayment>.AuditableEntityConfigurations(entity);
        }
    }
}