using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ShareholderConfiguration : IEntityTypeConfiguration<Shareholder>
    {
        public void Configure(EntityTypeBuilder<Shareholder> entity)
        {
            entity.ToTable(nameof(Shareholder), "Accounting");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(s => s.OwnershipPercentage)
                .HasColumnName("OwnershipPercentage")
                .HasColumnType("decimal(5, 2)")
                .IsRequired();

            entity.Property(s => s.TotalInvestment)
                .HasColumnName("TotalInvestment")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.TotalDrawings)
                .HasColumnName("TotalDrawings")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(s => s.ContactInfo)
                .HasColumnName("ContactInfo")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(s => s.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(s => s.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(a => a.Attachment)
                .HasColumnName("Attachemnt")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.HasMany(s => s.Transactions)
                .WithOne(t => t.Shareholder)
                .HasForeignKey(t => t.ShareholderId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<Shareholder>.AuditableEntityConfigurations(entity);
        }
    }
}
