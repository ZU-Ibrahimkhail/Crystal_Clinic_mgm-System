using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class BranchDetailsConfiguration : IEntityTypeConfiguration<BranchDetails>
    {
        public void Configure(EntityTypeBuilder<BranchDetails> entity)
        {
            entity.ToTable(nameof(BranchDetails), "Look");
            entity.HasKey("Id");
            entity.Property(c => c.Id).HasColumnName("Id");
            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
            entity.HasOne(b=>b.Branch).WithMany().HasForeignKey(b => b.BranchId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired(true).HasColumnType("bit").HasDefaultValue(true);
            entity.Property(c => c.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.Title).HasColumnName("Title").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.HeaderNote).HasColumnName("HeaderNote").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.FooterNote).HasColumnName("FooterNote").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.PhoneNumbers).HasColumnName("PhoneNumbers").HasColumnType("nvarchar(max)").IsRequired(false);

        }
    }
}
