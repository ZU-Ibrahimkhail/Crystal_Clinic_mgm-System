using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> entity)
        {
            entity.ToTable(nameof(Doctor), "CrystalClinic");

            // Primary Key configuration
            entity.HasKey(d => d.doctorId);

            // Property configurations
            entity.Property(d => d.firstName)
                  .HasColumnName("FirstName")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(d => d.lastName)
                  .HasColumnName("LastName")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(d => d.specialty)
                  .HasColumnName("Specialty")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(d => d.contactInfo)
                  .HasColumnName("ContactInfo")
                  .HasColumnType("nvarchar(255)")
                  .IsRequired(false); // Optional field

            entity.Property(d => d.isAvailable)
                  .HasColumnName("IsAvailable")
                  .HasColumnType("bit")
                  .IsRequired();

            // Auditable properties (if required, you can configure them as well)
            EntityConfiguration<Doctor>.AuditableEntityConfigurations(entity);
        }
    }
}
