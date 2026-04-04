using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class PayrollComponentConfiguration : IEntityTypeConfiguration<PayrollComponent>
    {
        public void Configure(EntityTypeBuilder<PayrollComponent> entity)
        {
            entity.ToTable("PayrollComponent", "HR");
            entity.HasKey(x => x.ID);

            entity.Property(c => c.ID).HasColumnName("").HasColumnType("int").ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.Type).HasColumnName("Type").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.CalculationType).HasColumnName("CalculationType").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(500).IsRequired(false);
            entity.Property(c => c.ChartOfAccountId).HasColumnName("ChartOfAccountId").HasColumnType("int").IsRequired(false);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasMany(x => x.EmployeeComponents)
                .WithOne(x => x.Component)
                .HasForeignKey(f => f.ComponentId)
                .OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<PayrollComponent>.AuditableEntityConfigurations(entity);
        }
    }
}
