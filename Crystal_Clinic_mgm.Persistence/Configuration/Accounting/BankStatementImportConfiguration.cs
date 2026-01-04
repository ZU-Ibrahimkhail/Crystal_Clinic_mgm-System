using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class BankStatementImportConfiguration : IEntityTypeConfiguration<BankStatementImport>
    {
        public void Configure(EntityTypeBuilder<BankStatementImport> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.FileName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(b => b.FileFormat)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(b => b.OpeningBalance)
                .HasPrecision(18, 4);

            builder.Property(b => b.ClosingBalance)
                .HasPrecision(18, 4);

            builder.Property(b => b.ErrorMessage)
                .HasMaxLength(500);

            builder.HasMany(b => b.Lines)
                .WithOne(l => l.BankStatementImport)
                .HasForeignKey(l => l.BankStatementImportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Matches)
                .WithOne(m => m.BankStatementImport)
                .HasForeignKey(m => m.BankStatementImportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(b => b.BankAccountId);
            builder.HasIndex(b => b.ImportDate);
            builder.HasIndex(b => b.Status);
        }
    }
}
