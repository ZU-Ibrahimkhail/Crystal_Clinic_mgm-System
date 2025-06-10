using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> entity)
        {
            entity.ToTable(nameof(Patient), "CrystalClinic");

            // Primary Key configuration
            entity.HasKey(p => p.patientId);

            // Property configurations
            entity.Property(p => p.name)
                  .HasColumnName("Name")
                  .HasColumnType("nvarchar(200)")
                  .IsRequired();

            entity.Property(p => p.contactInfo)
                  .HasColumnName("ContactInfo")
                  .HasColumnType("nvarchar(255)")
                  .IsRequired();

            entity.Property(p => p.email)
                  .HasColumnName("Email")
                  .HasColumnType("nvarchar(200)")
                  .IsRequired(false); // Optional field for email

            // Auditable properties (if required, you can configure them as well)
            EntityConfiguration<Patient>.AuditableEntityConfigurations(entity);
        }
    }
}
