using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class TaxBracketConfiguration : IEntityTypeConfiguration<TaxBracket>
    {
        public void Configure(EntityTypeBuilder<TaxBracket> entity)
        {
            entity.ToTable("TaxBracket", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.TaxConfigurationId).HasColumnName("TaxConfigurationId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.TaxConfiguration)
                .WithMany(x => x.Brackets)
                .HasForeignKey(f => f.TaxConfigurationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(c => c.MinAmount).HasColumnName("MinAmount").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.MaxAmount).HasColumnName("MaxAmount").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.Percentage).HasColumnName("Percentage").HasColumnType("decimal(5,2)").IsRequired(true);
            entity.Property(c => c.FlatAmount).HasColumnName("FlatAmount").HasColumnType("decimal(18,2)").IsRequired(true);
            entity.Property(c => c.CalculationType).HasColumnName("CalculationType").HasColumnType("int").IsRequired(true).HasDefaultValue(1); // StepByStep
            entity.Property(c => c.BracketOrder).HasColumnName("BracketOrder").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);

            entity.HasIndex(e => new { e.TaxConfigurationId, e.BracketOrder });

            EntityConfiguration<TaxBracket>.AuditableEntityConfigurations(entity);
        }
    }
}
