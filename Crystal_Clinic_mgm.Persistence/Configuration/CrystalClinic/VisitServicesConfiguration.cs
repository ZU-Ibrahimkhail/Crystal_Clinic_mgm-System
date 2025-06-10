using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class VisitServicesConfiguration : IEntityTypeConfiguration<VisitServices>
    {
        public void Configure(EntityTypeBuilder<VisitServices> entity)
        {
            // Table configuration
            entity.ToTable("VisitServices", "CrystalClinic");

            // Primary Key configuration
            entity.HasKey(vs => vs.visitServiceId);

            // Foreign Key relationships
            entity.HasOne(vs => vs.service)
                  .WithMany() // Assuming each service is used by multiple visits
                  .HasForeignKey(vs => vs.serviceId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Properties Configuration
            entity.Property(vs => vs.startDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(vs => vs.totalSessions)
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(vs => vs.completedSessions)
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(vs => vs.pricePerSession)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(vs => vs.totalPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(vs => vs.paidAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(vs => vs.remainAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(vs => vs.nextSessionDate)
                  .HasColumnType("datetime")
                  .IsRequired(false);

            entity.Property(vs => vs.sessionStatus)
                  .HasColumnType("nvarchar(50)")
                  .IsRequired(false);

            entity.Property(vs => vs.discount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired(false);

            // Enum Property for Payment Status
            entity.Property(vs => vs.paymentStatus)
                  .HasConversion<string>()
                  .IsRequired();

            // Optional: Add more constraints as needed
            // e.g. entity.Property(vs => vs.totalSessions).HasDefaultValue(0);
        }
    }
}
