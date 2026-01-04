using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class BudgetLineConfiguration : IEntityTypeConfiguration<BudgetLine>
    {
        public void Configure(EntityTypeBuilder<BudgetLine> entity)
        {
            entity.ToTable(nameof(BudgetLine), "Accounting");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.BudgetId)
                .HasColumnName("BudgetId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(b => b.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(b => b.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(b => b.PeriodId)
                .HasColumnName("PeriodId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(b => b.BudgetedAmount)
                .HasColumnName("BudgetedAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(b => b.ActualAmount)
                .HasColumnName("ActualAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(b => b.Variance)
                .HasColumnName("Variance")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(b => b.Budget)
                .WithMany(x => x.BudgetLines)
                .HasForeignKey(b => b.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.ChartOfAccount)
                .WithMany()
                .HasForeignKey(b => b.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Branch)
                .WithMany()
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<BudgetLine>.AuditableEntityConfigurations(entity);
        }
    }
}
