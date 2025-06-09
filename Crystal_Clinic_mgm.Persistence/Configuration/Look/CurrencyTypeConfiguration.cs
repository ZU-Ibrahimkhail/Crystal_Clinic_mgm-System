using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class CurrencyTypeConfiguration : IEntityTypeConfiguration<CurrencyType>
    {
        public void Configure(EntityTypeBuilder<CurrencyType> entity)
        {
            entity.ToTable(nameof(CurrencyType), "Look");
            EntityConfigurations<CurrencyType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
