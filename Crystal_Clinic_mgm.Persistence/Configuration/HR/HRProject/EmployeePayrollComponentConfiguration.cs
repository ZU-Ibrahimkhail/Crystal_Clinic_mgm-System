using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class EmployeePayrollComponentConfiguration : IEntityTypeConfiguration<EmployeePayrollComponent>
    {
        public void Configure(EntityTypeBuilder<EmployeePayrollComponent> entity)
        {
            entity.ToTable("EmployeePayrollComponent", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.PayrollComponents)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ComponentId).HasColumnName("ComponentId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Component)
                .WithMany(x => x.EmployeeComponents)
                .HasForeignKey(f => f.ComponentId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.EffectiveDate).HasColumnName("EffectiveDate").HasColumnType("date").IsRequired(true);
            entity.Property(c => c.EndDate).HasColumnName("EndDate").HasColumnType("date").IsRequired(false);

            entity.Property(c => c.OverrideAmount).HasColumnName("OverrideAmount").HasColumnType("decimal(18,2)").IsRequired(false);
            entity.Property(c => c.Notes).HasColumnName("Notes").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasIndex(e => e.EmployeeId);
            entity.HasIndex(e => new { e.EmployeeId, e.ComponentId });

            EntityConfiguration<EmployeePayrollComponent>.AuditableEntityConfigurations(entity);
        }
    }
}
