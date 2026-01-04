using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class RecurringJournalTemplateConfiguration : IEntityTypeConfiguration<RecurringJournalTemplate>
    {
        public void Configure(EntityTypeBuilder<RecurringJournalTemplate> entity)
        {
            entity.ToTable(nameof(RecurringJournalTemplate), "Accounting");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.TemplateName)
                .HasColumnName("TemplateName")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(r => r.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(r => r.Frequency)
                .HasColumnName("Frequency")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(r => r.NextRunDate)
                .HasColumnName("NextRunDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(r => r.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(r => r.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.HasMany(r => r.Lines)
                .WithOne(l => l.RecurringJournalTemplate)
                .HasForeignKey(l => l.RecurringJournalTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<RecurringJournalTemplate>.AuditableEntityConfigurations(entity);
        }
    }
}
