using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> entity)
        {
            entity.ToTable(nameof(JournalEntryLine), "Accounting");

            entity.HasKey(j => j.Id);

            entity.Property(j => j.JournalEntryId)
                .HasColumnName("JournalEntryId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(j => j.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(j => j.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(j => j.DebitAmount)
                .HasColumnName("DebitAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(j => j.CreditAmount)
                .HasColumnName("CreditAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(j => j.CurrencyId)
                .HasColumnName("CurrencyId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(j => j.ExchangeRate)
                .HasColumnName("ExchangeRate")
                .HasColumnType("decimal(18, 6)")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(j => j.AmountInBaseCurrency)
                .HasColumnName("AmountInBaseCurrency")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(j => j.JournalEntry)
                .WithMany(e => e.JournalEntryLines)
                .HasForeignKey(j => j.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(j => j.ChartOfAccount)
                .WithMany()
                .HasForeignKey(j => j.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(j => j.Currency)
                .WithMany()
                .HasForeignKey(j => j.CurrencyId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<JournalEntryLine>.AuditableEntityConfigurations(entity);
        }
    }
}
