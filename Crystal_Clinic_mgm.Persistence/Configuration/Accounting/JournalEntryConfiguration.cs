using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> entity)
        {
            entity.ToTable(nameof(JournalEntry), "Accounting");

            entity.HasKey(j => j.Id);

            entity.Property(j => j.EntryNumber)
                .HasColumnName("EntryNumber")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(j => j.EntryDate)
                .HasColumnName("EntryDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(j => j.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(j => j.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(JournalEntryStatus.Unposted);

            entity.Property(j => j.ApprovedBy)
                .HasColumnName("ApprovedBy")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired(false);

            entity.Property(j => j.ApprovedDate)
                .HasColumnName("ApprovedDate")
                .HasColumnType("datetime")
                .IsRequired(false);

            entity.Property(j => j.ReferenceNumber)
                .HasColumnName("ReferenceNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired(false)
                .HasMaxLength(100);

            entity.Property(j => j.ReferenceType)
                .HasColumnName("ReferenceType")
                .HasColumnType("nvarchar(50)")
                .IsRequired(false)
                .HasMaxLength(50);

            entity.Property(j => j.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(j => j.Branch)
                .WithMany()
                .HasForeignKey(j => j.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(j => j.JournalEntryLines)
                .WithOne(l => l.JournalEntry)
                .HasForeignKey(l => l.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(j => j.GeneralLedgerEntries)
                .WithOne(g => g.JournalEntry)
                .HasForeignKey(g => g.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(j => j.EntryNumber).IsUnique();

            EntityConfiguration<JournalEntry>.AuditableEntityConfigurations(entity);
        }
    }
}
