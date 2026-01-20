using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class ContractDetailsConfiguration : IEntityTypeConfiguration<ContractDetails>
    {
        public void Configure(EntityTypeBuilder<ContractDetails> entity)
        {
            entity.ToTable("ContractDetails", "HR");
            entity.HasKey("ID");
            entity.Property(c => c.EmployeeProfileId).HasColumnName("EmployeeProfileId").HasColumnType("int").IsRequired(true);
            entity.HasOne<EmployeeProfile>("EmployeeProfile").WithMany().HasForeignKey(f => f.EmployeeProfileId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.ContractTypeId).HasColumnName("ContractTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne<ContractType>("ContractType").WithMany().HasForeignKey(f => f.ContractTypeId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.PositionTitleId).HasColumnName("PositionTitleId").HasColumnType("int").IsRequired(true);
            entity.HasOne<PositionTitle>("PositionTitle").WithMany().HasForeignKey(f => f.PositionTitleId).OnDelete(DeleteBehavior.NoAction);


            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(f => f.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);
            
            entity.Property(c => c.SalaryAmount).HasColumnName("SalaryAmount").HasColumnType("real").IsRequired(true);

            entity.Property(c => c.StartDate).HasColumnName("StartDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.EndDate).HasColumnName("EndDate").HasColumnType("DateTime").IsRequired(false).HasDefaultValue(null);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true);
            entity.Property(c => c.AttachmentPath)
                .HasColumnName("AttachmentPath")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                    )
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);
                    entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true);
                    entity.HasOne(x => x.Branch).WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<ContractDetails>.AuditableEntityConfigurations(entity);

        }
    }
}
