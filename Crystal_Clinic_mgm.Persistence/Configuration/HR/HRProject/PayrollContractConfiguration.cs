using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class PayrollContractConfiguration : IEntityTypeConfiguration<PayrollContract>
    {
        public void Configure(EntityTypeBuilder<PayrollContract> entity)
        {
            entity.ToTable("PayrollContract", "HR");
            entity.HasKey(x => x.ID);

            entity.Property(c => c.ID).HasColumnName("ID").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeProfileId).HasColumnName("EmployeeProfileId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.EmployeeProfile)
                .WithMany(x => x.PayrollContracts)
                .HasForeignKey(f => f.EmployeeProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ContractTypeId).HasColumnName("ContractTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.ContractType)
                .WithMany()
                .HasForeignKey(f => f.ContractTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.PositionTitleId).HasColumnName("PositionTitleId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.PositionTitle)
                .WithMany(x => x.PayrollContracts)
                .HasForeignKey(f => f.PositionTitleId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true);
            entity.HasOne<Branch>().WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType)
                .WithMany()
                .HasForeignKey(f => f.CurrencyTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.BaseSalary).HasColumnName("BaseSalary").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.StartDate).HasColumnName("StartDate").HasColumnType("datetime").IsRequired(true);
            entity.Property(c => c.EndDate).HasColumnName("EndDate").HasColumnType("datetime").IsRequired(false);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);
            entity.Property(c => c.AttachmentPath).HasColumnName("AttachmentPath").HasColumnType("nvarchar(max)").IsRequired(false);

            // Phase 1 Enhancements
            entity.Property(c => c.Conditions).HasColumnName("Conditions").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.PayCycle).HasColumnName("PayCycle").HasColumnType("int").IsRequired(true).HasDefaultValue(PayCycle.Monthly); // Monthly
            entity.Property(c => c.InsuranceDetails).HasColumnName("InsuranceDetails").HasColumnType("nvarchar(max)").IsRequired(false);

            EntityConfiguration<PayrollContract>.AuditableEntityConfigurations(entity);
        }
    }
}
