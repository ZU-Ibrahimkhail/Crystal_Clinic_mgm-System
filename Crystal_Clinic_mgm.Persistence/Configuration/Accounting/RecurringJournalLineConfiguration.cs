using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class RecurringJournalLineConfiguration : IEntityTypeConfiguration<RecurringJournalLine>
    {
        public void Configure(EntityTypeBuilder<RecurringJournalLine> entity)
        {
            entity.ToTable(nameof(RecurringJournalLine), "Accounting");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.RecurringJournalTemplateId)
                .HasColumnName("RecurringJournalTemplateId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(r => r.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(r => r.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(r => r.DebitAmount)
                .HasColumnName("DebitAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(r => r.CreditAmount)
                .HasColumnName("CreditAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(r => r.RecurringJournalTemplate)
                .WithMany(t => t.Lines)
                .HasForeignKey(r => r.RecurringJournalTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.ChartOfAccount)
                .WithMany()
                .HasForeignKey(r => r.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<RecurringJournalLine>.AuditableEntityConfigurations(entity);
        }
    }
}
