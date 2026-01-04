using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class BankMatchConfiguration : IEntityTypeConfiguration<BankMatch>
    {
        public void Configure(EntityTypeBuilder<BankMatch> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MatchedAmount)
                .HasPrecision(18, 4);

            builder.Property(m => m.Notes)
                .HasMaxLength(500);

            builder.HasOne(m => m.BankStatementImport)
                .WithMany(b => b.Matches)
                .HasForeignKey(m => m.BankStatementImportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.BankStatementLine)
                .WithMany(l => l.Matches)
                .HasForeignKey(m => m.BankStatementLineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.GeneralLedger)
                .WithMany()
                .HasForeignKey(m => m.GeneralLedgerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.JournalEntry)
                .WithMany()
                .HasForeignKey(m => m.JournalEntryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(m => m.BankStatementImportId);
            builder.HasIndex(m => m.BankStatementLineId);
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.MatchDate);
        }
    }
}
