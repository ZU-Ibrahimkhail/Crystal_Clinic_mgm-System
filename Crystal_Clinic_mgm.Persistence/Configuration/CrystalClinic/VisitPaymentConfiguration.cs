using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Crystal_Clinic
{
    public class VisitPaymentConfiguration : IEntityTypeConfiguration<VisitPayment>
    {
        public void Configure(EntityTypeBuilder<VisitPayment> entity)
        {
            // Define table name and schema
            entity.ToTable(nameof(VisitPayment), "CrystalClinic");

            // Define primary key
            entity.HasKey(vp => vp.visitPaymentId);

            // Define properties and their types
            entity.Property(vp => vp.visitPaymentId)
                .HasColumnName("VisitPaymentId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(vp => vp.visitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(vp => vp.serviceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(vp => vp.CurrencyTypeId)
                .HasColumnName("CurrencyTypeId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(vp => vp.sessionNumber)
                .HasColumnName("SessionNumber")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(vp => vp.PaymentToAFNExchangeRate)
                  .HasColumnName("PaymentToAFNExchangeRate")
                  .HasColumnType("decimal")
                  .IsRequired();


            entity.Property(vp => vp.amountPaid)
                .HasColumnName("AmountPaid")
                .HasColumnType("decimal")
                .IsRequired();

            entity.Property(vp => vp.paymentStatus)
                .HasColumnName("PaymentStatus")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(vp => vp.paymentDate)
                .HasColumnName("PaymentDate")
                .HasColumnType("DateTime")
                .IsRequired();

            // Define foreign key relationships
            entity.HasOne(vp => vp.service)
                .WithMany()  // Assuming no navigation property on Service side
                .HasForeignKey(vp => vp.serviceId)
                .OnDelete(DeleteBehavior.SetNull); // In case ServiceId is nullable, do not delete related VisitPayment

            entity.HasOne(vp => vp.CurrencyType)
                .WithMany()  // Assuming no navigation property on Service side
                .HasForeignKey(vp => vp.CurrencyTypeId)
                .OnDelete(DeleteBehavior.SetNull); // In case ServiceId is nullable, do not delete related VisitPayment

            // Additional audit and timestamp properties inherited from AuditableEntity class
            // Add those properties here if needed (AuditableEntity should include CreatedOn, CreatedBy, etc.)
        }
    }
}
