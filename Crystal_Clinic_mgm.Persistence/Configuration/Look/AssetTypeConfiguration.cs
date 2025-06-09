using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class AssetTypeConfiguration : IEntityTypeConfiguration<AssetType>
    {
        public void Configure(EntityTypeBuilder<AssetType> entity)
        {
            entity.ToTable(nameof(AssetType), "Look");
            EntityConfigurations<AssetType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
