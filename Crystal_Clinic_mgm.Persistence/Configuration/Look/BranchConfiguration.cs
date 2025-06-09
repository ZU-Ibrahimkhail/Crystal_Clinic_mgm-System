using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.ToTable(nameof(Branch), "Look");
            entity.HasKey("ID");
            entity.Property(c => c.ID).HasColumnName("ID");
            entity.HasOne<Branch>("Parent").WithMany().HasForeignKey(r => r.ParentId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.EnglishName).HasColumnName("EnglishName").HasColumnType("nvarchar").HasMaxLength(500).IsRequired(true);
            entity.Property(c => c.PashtoName).HasColumnName("PashtoName").HasColumnType("nvarchar").HasMaxLength(500).IsRequired(true);
            entity.Property(c => c.DariName).HasColumnName("DariName").HasColumnType("nvarchar").HasMaxLength(500).IsRequired(true);
            entity.Property(c => c.Code).HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired(true).HasColumnType("bit").HasDefaultValue(true);
            entity.Property(c => c.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").IsRequired(false);

            EntityConfiguration<Branch>.AuditableEntityConfigurations(entity);
        }
    }
}
