using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Applications>
    {
        public void Configure(EntityTypeBuilder<Applications> entity)
        {
            entity.ToTable("Applications", "dbo");
            entity.HasKey(pk => pk.ID);
            entity.Property(c => c.ID).ValueGeneratedOnAdd();
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}
