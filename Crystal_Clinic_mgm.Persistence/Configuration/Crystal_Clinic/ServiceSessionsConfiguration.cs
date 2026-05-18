using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Crystal_Clinic
{
    public class ServiceSessionsConfiguration : IEntityTypeConfiguration<ServiceSessions>
    {
        public void Configure(EntityTypeBuilder<ServiceSessions> entity)
        {
            entity.ToTable(nameof(ServiceSessions), "CrystalClinic");

            entity.HasKey(ss => ss.Id);

            entity.Property(ss => ss.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(ss => ss.visitServiceId)
                .HasColumnName("VisitServiceId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ss => ss.visitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ss => ss.serviceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(ss => ss.serviceName)
                .HasColumnName("ServiceName")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            entity.Property(ss => ss.patientName)
                .HasColumnName("PatientName")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            entity.Property(ss => ss.contactInfo)
                .HasColumnName("ContactInfo")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            entity.Property(ss => ss.sessionNumber)
                .HasColumnName("SessionNumber")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ss => ss.PriceInAFN)
                .HasColumnName("PriceInAFN")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(ss => ss.IsImplemented)
                .HasColumnName("IsImplemented")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(ss => ss.ImplementationDate)
                .HasColumnName("ImplementationDate")
                .HasColumnType("datetime2")
                .IsRequired(false);

            entity.Property(ss => ss.ImplementorEmployeeId)
                .HasColumnName("ImplementorEmployeeId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(ss => ss.ImplementorEmployee)
                .WithMany()
                .HasForeignKey(ss => ss.ImplementorEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
