using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
    {
        public void Configure(EntityTypeBuilder<GeneralLedger> entity)
        {
            entity.ToTable(nameof(GeneralLedger), "Accounting");

            entity.HasKey(g => g.Id);

            entity.Property(g => g.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(g => g.JournalEntryId)
                .HasColumnName("JournalEntryId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(g => g.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(g => g.TransactionDate)
                .HasColumnName("TransactionDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(g => g.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(g => g.DebitAmount)
                .HasColumnName("DebitAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(g => g.CreditAmount)
                .HasColumnName("CreditAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(g => g.Balance)
                .HasColumnName("Balance")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(g => g.ChartOfAccount)
                .WithMany(c => c.GeneralLedgerEntries)
                .HasForeignKey(g => g.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.JournalEntry)
                .WithMany(j => j.GeneralLedgerEntries)
                .HasForeignKey(g => g.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Branch)
                .WithMany()
                .HasForeignKey(g => g.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(g => new { g.ChartOfAccountId, g.TransactionDate });

            EntityConfiguration<GeneralLedger>.AuditableEntityConfigurations(entity);
        }
    }
}
