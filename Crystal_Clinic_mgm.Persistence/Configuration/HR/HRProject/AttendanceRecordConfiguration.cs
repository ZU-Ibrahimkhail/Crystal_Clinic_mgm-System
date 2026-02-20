using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> entity)
        {
            entity.ToTable("AttendanceRecord", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ShiftId).HasColumnName("ShiftId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.Shift)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(f => f.ShiftId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.CheckIn).HasColumnName("CheckIn").HasColumnType("datetime").IsRequired(true);
            entity.Property(c => c.CheckOut).HasColumnName("CheckOut").HasColumnType("datetime").IsRequired(false);

            entity.Property(c => c.Status).HasColumnName("Status").HasColumnType("int").IsRequired(true).HasDefaultValue(1); // Present
            entity.Property(c => c.WorkingHours).HasColumnName("WorkingHours").HasColumnType("decimal(5,2)").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.OvertimeHours).HasColumnName("OvertimeHours").HasColumnType("decimal(5,2)").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.Notes).HasColumnName("Notes").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.Property(c => c.AttendanceDate).HasColumnName("AttendanceDate").HasColumnType("date").IsRequired(true);

            entity.HasIndex(e => new { e.EmployeeId, e.AttendanceDate }).IsUnique();

            EntityConfiguration<AttendanceRecord>.AuditableEntityConfigurations(entity);
        }
    }
}
