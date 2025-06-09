using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> entity)
        {
            entity.ToTable("ContractType", "HRLooks");
            EntityConfigurations<ContractType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
