using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> entity)
        {
            // Table Name
            entity.ToTable(nameof(Supplier), "BranchStock");

            // Primary Key
            entity.HasKey(s => s.Id);

            // Properties configurations
            entity.Property(s => s.Name)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.ContactPerson)
                .HasColumnName("ContactPerson")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.ContactInfo)
                .HasColumnName("ContactInfo")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(s => s.Email)
                .HasColumnName("Email")
                .HasMaxLength(100);

            entity.Property(s => s.Address)
                .HasColumnName("Address")
                .HasMaxLength(200);

            // Auditable entity fields configuration
            EntityConfiguration<Supplier>.AuditableEntityConfigurations(entity);
        }
    }
}