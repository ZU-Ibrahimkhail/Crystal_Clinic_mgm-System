using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> entity)
        {
            entity.ToTable("Shift", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.StartTime).HasColumnName("StartTime").HasColumnType("time").IsRequired(true);
            entity.Property(c => c.EndTime).HasColumnName("EndTime").HasColumnType("time").IsRequired(true);
            entity.Property(c => c.GracePeriodMinutes).HasColumnName("GracePeriodMinutes").HasColumnType("int").IsRequired(true).HasDefaultValue(5);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasMany(x => x.AttendanceRecords)
                .WithOne(x => x.Shift)
                .HasForeignKey(f => f.ShiftId)
                .OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<Shift>.AuditableEntityConfigurations(entity);
        }
    }
}
