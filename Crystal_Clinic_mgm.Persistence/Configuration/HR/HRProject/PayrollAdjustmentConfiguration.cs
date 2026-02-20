using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class PayrollAdjustmentConfiguration : IEntityTypeConfiguration<PayrollAdjustment>
    {
        public void Configure(EntityTypeBuilder<PayrollAdjustment> entity)
        {
            entity.ToTable("PayrollAdjustment", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.Type).HasColumnName("Type").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.Category).HasColumnName("Category").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.AdjustmentDate).HasColumnName("AdjustmentDate").HasColumnType("date").IsRequired(true);

            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.ReferenceNumber).HasColumnName("ReferenceNumber").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(false);
            entity.Property(c => c.IsProcessed).HasColumnName("IsProcessed").HasColumnType("bit").IsRequired(true).HasDefaultValue(false);

            entity.HasIndex(e => new { e.EmployeeId, e.AdjustmentDate });
            entity.HasIndex(e => e.IsProcessed);

            EntityConfiguration<PayrollAdjustment>.AuditableEntityConfigurations(entity);
        }
    }
}
