using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class EquityTransactionConfiguration : IEntityTypeConfiguration<EquityTransaction>
    {
        public void Configure(EntityTypeBuilder<EquityTransaction> entity)
        {
            entity.ToTable(nameof(EquityTransaction), "Accounting");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.ShareholderId)
                .HasColumnName("ShareholderId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("Type")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(e => e.TransactionDate)
                .HasColumnName("TransactionDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(e => e.Reference)
                .HasColumnName("Reference")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.HasOne(e => e.Shareholder)
                .WithMany(s => s.Transactions)
                .HasForeignKey(e => e.ShareholderId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<EquityTransaction>.AuditableEntityConfigurations(entity);
        }
    }
}
