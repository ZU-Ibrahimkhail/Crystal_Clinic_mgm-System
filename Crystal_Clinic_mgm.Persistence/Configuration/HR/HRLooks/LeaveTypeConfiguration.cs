using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> entity)
        {
            entity.ToTable("LeaveType", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.MaxDaysPerYear).HasColumnName("MaxDaysPerYear").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.IsPaid).HasColumnName("IsPaid").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);
            entity.Property(c => c.AllowCarryover).HasColumnName("AllowCarryover").HasColumnType("bit").IsRequired(true).HasDefaultValue(false);
            entity.Property(c => c.MaxCarryoverDays).HasColumnName("MaxCarryoverDays").HasColumnType("int").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(500).IsRequired(false);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasMany(x => x.LeaveRequests)
                .WithOne(x => x.LeaveType)
                .HasForeignKey(f => f.LeaveTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<LeaveType>.AuditableEntityConfigurations(entity);
        }
    }
}
