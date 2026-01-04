using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> entity)
        {
            entity.ToTable(nameof(Budget), "Accounting");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.BudgetName)
                .HasColumnName("BudgetName")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(b => b.FiscalYear)
                .HasColumnName("FiscalYear")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(b => b.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(b => b.ApprovedBy)
                .HasColumnName("ApprovedBy")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired(false);

            entity.Property(b => b.ApprovedDate)
                .HasColumnName("ApprovedDate")
                .HasColumnType("datetime")
                .IsRequired(false);

            entity.Property(b => b.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(b => b.Branch)
                .WithMany()
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(b => b.BudgetLines)
                .WithOne(l => l.Budget)
                .HasForeignKey(l => l.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<Budget>.AuditableEntityConfigurations(entity);
        }
    }
}
