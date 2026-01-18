using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class BankStatementLineConfiguration : IEntityTypeConfiguration<BankStatementLine>
    {
        public void Configure(EntityTypeBuilder<BankStatementLine> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.ReferenceNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(l => l.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(l => l.Amount)
                .HasPrecision(18, 4);

            builder.Property(l => l.TransactionType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.RunningBalance)
                .HasPrecision(18, 4);

            builder.Property(l => l.BankCode)
                .HasMaxLength(50);

            builder.HasOne(l => l.BankStatementImport)
                .WithMany(b => b.Lines)
                .HasForeignKey(l => l.BankStatementImportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Matches)
                .WithOne(m => m.BankStatementLine)
                .HasForeignKey(m => m.BankStatementLineId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(l => l.BankStatementImportId);
            builder.HasIndex(l => l.TransactionDate);
            builder.HasIndex(l => l.IsMatched);
        }
    }
}
