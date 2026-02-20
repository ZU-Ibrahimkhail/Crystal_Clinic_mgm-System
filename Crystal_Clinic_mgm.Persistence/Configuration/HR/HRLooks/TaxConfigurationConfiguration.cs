using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class TaxConfigurationConfiguration : IEntityTypeConfiguration<TaxConfiguration>
    {
        public void Configure(EntityTypeBuilder<TaxConfiguration> entity)
        {
            entity.ToTable("TaxConfiguration", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.TaxType).HasColumnName("TaxType").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasMany(x => x.Brackets)
                .WithOne(x => x.TaxConfiguration)
                .HasForeignKey(f => f.TaxConfigurationId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<TaxConfiguration>.AuditableEntityConfigurations(entity);
        }
    }
}
