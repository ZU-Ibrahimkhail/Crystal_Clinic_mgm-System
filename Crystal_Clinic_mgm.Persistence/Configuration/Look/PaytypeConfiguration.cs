using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class PaytypeConfiguration : IEntityTypeConfiguration<PayType>
    {
        public void Configure(EntityTypeBuilder<PayType> entity)
        {
            entity.ToTable(nameof(PayType), "Look");
            EntityConfigurations<PayType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
