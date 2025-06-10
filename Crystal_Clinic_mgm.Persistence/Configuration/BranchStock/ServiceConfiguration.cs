using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Crystal_Clinic
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> entity)
        {
            // Table configuration
            entity.ToTable("Service", "Stock");

            // Primary Key configuration
            entity.HasKey(s => s.ServiceId);

            // Properties configuration
            entity.Property(s => s.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(255)")
                .IsRequired();

            entity.Property(s => s.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(s => s.sessionRate)
                .HasColumnName("SessionRate")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(s => s.ImagePath)
                .HasColumnName("ImagePath")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            // Optional: You can also add other configurations, like indexes, defaults, etc.
            // For example, if you want to add an index to `Name` for better search performance:
            entity.HasIndex(s => s.Name).HasDatabaseName("IX_Service_Name");
            EntityConfiguration<Service>.AuditableEntityConfigurations(entity);
        }
    }
}
