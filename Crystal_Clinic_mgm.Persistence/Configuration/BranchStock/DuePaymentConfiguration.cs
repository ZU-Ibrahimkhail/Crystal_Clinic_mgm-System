using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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

            // Foreign Key relationships
            entity.HasOne(dp => dp.SupplierDue)
                .WithMany(sd => sd.Payments)
                .HasForeignKey(dp => dp.SupplierDueId)
                .OnDelete(DeleteBehavior.Cascade);

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

            entity.Property(dp => dp.AmountPaid)
                .HasColumnName("AmountPaid")
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

            entity.Property(c => c.AttachmentPath)
                    .HasColumnName("AttachmentPath")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                        )
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);

            // Auditable entity fields configuration
            EntityConfiguration<DuePayment>.AuditableEntityConfigurations(entity);
        }
    }
}