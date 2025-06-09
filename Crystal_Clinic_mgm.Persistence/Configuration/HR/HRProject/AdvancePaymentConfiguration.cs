using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class AdvancePaymentConfiguration : IEntityTypeConfiguration<AdvancePayment>
    {
        public void Configure(EntityTypeBuilder<AdvancePayment> entity)
        {
            entity.ToTable(nameof(AdvancePayment), "HR");
            entity.HasKey(x => x.ID);

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee).WithMany().HasForeignKey(f => f.EmployeeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.PayTypeId).HasColumnName("PayTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.PayType).WithMany().HasForeignKey(f => f.PayTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(f => f.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.MainAccountId).HasColumnName("MainAccountId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne(x => x.MainAccount).WithMany().HasForeignKey(f => f.MainAccountId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.AdvanceDate).HasColumnName("AdvanceDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.AdvanceAmount).HasColumnName("AdvanceAmount").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.RemainingBalance).HasColumnName("RemainingBalance").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.EachInstallmentAmount).HasColumnName("EachInstallmentAmount").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.PayedBy).HasColumnName("PayedBy").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);


            EntityConfiguration<AdvancePayment>.AuditableEntityConfigurations(entity);
        }
    }
}
