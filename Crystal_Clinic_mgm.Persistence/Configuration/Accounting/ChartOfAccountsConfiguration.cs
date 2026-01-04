using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ChartOfAccountsConfiguration : IEntityTypeConfiguration<ChartOfAccounts>
    {
        public void Configure(EntityTypeBuilder<ChartOfAccounts> entity)
        {
            entity.ToTable(nameof(ChartOfAccounts), "Accounting");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.AccountCode)
                .HasColumnName("AccountCode")
                .HasColumnType("nvarchar(20)")
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.AccountName)
                .HasColumnName("AccountName")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(c => c.AccountType)
                .HasColumnName("AccountType")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(c => c.AccountCategory)
                .HasColumnName("AccountCategory")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(c => c.NormalBalance)
                .HasColumnName("NormalBalance")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(c => c.IsSystemAccount)
                .HasColumnName("IsSystemAccount")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(c => c.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(c => c.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(c => c.ParentAccountId)
                .HasColumnName("ParentAccountId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(c => c.ParentAccount)
                .WithMany(c => c.ChildAccounts)
                .HasForeignKey(c => c.ParentAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(c => c.JournalEntryLines)
                .WithOne(j => j.ChartOfAccount)
                .HasForeignKey(j => j.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.GeneralLedgerEntries)
                .WithOne(g => g.ChartOfAccount)
                .HasForeignKey(g => g.ChartOfAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(c => c.AccountCode).IsUnique();

            EntityConfiguration<ChartOfAccounts>.AuditableEntityConfigurations(entity);
        }
    }
}
