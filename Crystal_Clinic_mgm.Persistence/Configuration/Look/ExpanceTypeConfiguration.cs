using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
    {
        public void Configure(EntityTypeBuilder<ExpenseType> entity)
        {
            entity.ToTable(nameof(ExpenseType), "Look");
            EntityConfigurations<ExpenseType>.LookAuditableEntityConfigurations(entity);
        }
    }
}
