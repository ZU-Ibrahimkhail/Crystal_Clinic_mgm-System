using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class PositionTitleConfiguration : IEntityTypeConfiguration<PositionTitle>
    {
        public void Configure(EntityTypeBuilder<PositionTitle> entity)
        {
            entity.ToTable("PositionTitle", "HRLooks");
            entity.HasKey(x => x.ID);
            entity.Property(c => c.ID).HasColumnName("Id");
            entity.Property(c => c.Title).HasColumnName("Title").HasMaxLength(100).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.JobDescription).HasColumnName("JobDescription").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true).HasDefaultValue(1);
            entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);

            // Phase 1 Enhancements
            entity.Property(c => c.ReportsToId).HasColumnName("ReportsToId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.ReportsTo)
                .WithMany()
                .HasForeignKey(f => f.ReportsToId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.JobGrade).HasColumnName("JobGrade").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            entity.Property(c => c.MinSalary).HasColumnName("MinSalary").HasColumnType("decimal(18,2)").IsRequired(true).HasDefaultValue(0);
            entity.Property(c => c.MaxSalary).HasColumnName("MaxSalary").HasColumnType("decimal(18,2)").IsRequired(true).HasDefaultValue(0);

            entity.HasMany(x => x.PayrollContracts)
                .WithOne(x => x.PositionTitle)
                .HasForeignKey(f => f.PositionTitleId)
                .OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<PositionTitle>.AuditableEntityConfigurations(entity);
        }
    }
}

