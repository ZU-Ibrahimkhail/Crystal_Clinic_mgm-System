using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class LeaveCarryoverConfiguration : IEntityTypeConfiguration<LeaveCarryover>
    {
        public void Configure(EntityTypeBuilder<LeaveCarryover> entity)
        {
            entity.ToTable("LeaveCarryover", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.LeaveTypeId).HasColumnName("LeaveTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.LeaveType)
                .WithMany()
                .HasForeignKey(f => f.LeaveTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.CarryoverYear).HasColumnName("CarryoverYear").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.RemainingDays).HasColumnName("RemainingDays").HasColumnType("decimal(5,2)").IsRequired(true);
            entity.Property(c => c.UsedDays).HasColumnName("UsedDays").HasColumnType("decimal(5,2)").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.ExpirationDays).HasColumnName("ExpirationDays").HasColumnType("decimal(5,2)").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("date").IsRequired(true);
            entity.Property(c => c.Notes).HasColumnName("Notes").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.HasIndex(e => new { e.EmployeeId, e.LeaveTypeId, e.CarryoverYear }).IsUnique();

            EntityConfiguration<LeaveCarryover>.AuditableEntityConfigurations(entity);
        }
    }
}
