using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class LoanTypeConfiguration : IEntityTypeConfiguration<LoanType>
    {
        public void Configure(EntityTypeBuilder<LoanType> entity)
        {
            entity.ToTable(nameof(LoanType), "Look");
            EntityConfigurations<LoanType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
