using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class PartnersConfiguration : IEntityTypeConfiguration<Partners>
    {
        public void Configure(EntityTypeBuilder<Partners> entity)
        {
            entity.ToTable(nameof(Partners), "HRLooks");
            entity.HasKey(x => x.ID);
            entity.Property(c => c.NameInEnglish).HasColumnName("NameInEnglish").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            entity.Property(c => c.NameInPashto).HasColumnName("NameInPashto").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            entity.Property(c => c.Phone).HasColumnName("Phone").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.Email).HasColumnName("Email").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
            EntityConfiguration<Partners>.AuditableEntityConfigurations(entity);
        }
    }
}
