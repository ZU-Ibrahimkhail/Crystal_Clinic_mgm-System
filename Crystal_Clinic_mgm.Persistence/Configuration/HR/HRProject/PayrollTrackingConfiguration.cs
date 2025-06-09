using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class PayrollTrackingConfiguration : IEntityTypeConfiguration<PayrollTracking>
    {
        public void Configure(EntityTypeBuilder<PayrollTracking> entity)
        {
            entity.ToTable(nameof(PayrollTracking), "HR");
            entity.HasKey(x => x.ID);

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee).WithMany().HasForeignKey(f => f.EmployeeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ContractDetailsId).HasColumnName("ContractDetailsId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.ContractDetails).WithMany().HasForeignKey(f => f.ContractDetailsId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.PayTypeId).HasColumnName("PayTypeId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.PayType).WithMany().HasForeignKey(f => f.PayTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.Branch).WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(f => f.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.Date).HasColumnName("Date").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.BaseSalary).HasColumnName("BaseSalary").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.AdvanceDeduction).HasColumnName("AdvanceDeduction").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.NetSalary).HasColumnName("NetSalary").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.PayedBy).HasColumnName("PayedBy").HasColumnType("UNIQUEIDENTIFIER").IsRequired(false);
            entity.Property(c => c.IsPayed).HasColumnName("IsPayed").HasColumnType("bit").HasDefaultValue(false);


            EntityConfiguration<PayrollTracking>.AuditableEntityConfigurations(entity);
        }
    }
}
