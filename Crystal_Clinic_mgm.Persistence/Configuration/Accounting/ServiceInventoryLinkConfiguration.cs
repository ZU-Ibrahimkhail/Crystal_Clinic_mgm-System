using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ServiceInventoryLinkConfiguration : IEntityTypeConfiguration<ServiceInventoryLink>
    {
        public void Configure(EntityTypeBuilder<ServiceInventoryLink> entity)
        {
            entity.ToTable(nameof(ServiceInventoryLink), "Accounting");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.ServiceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.InventoryItemId)
                .HasColumnName("InventoryItemId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.QuantityRequired)
                .HasColumnName("QuantityRequired")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(s => s.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.HasIndex(s => new { s.ServiceId, s.InventoryItemId }).IsUnique();

            EntityConfiguration<ServiceInventoryLink>.AuditableEntityConfigurations(entity);
        }
    }
}
