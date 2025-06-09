using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class UserAllowedDocTypesSecurityLevelsConfiguration : IEntityTypeConfiguration<UserAllowedDocTypesSecurityLevels>
    {
        public void Configure(EntityTypeBuilder<UserAllowedDocTypesSecurityLevels> entity)
        {
            entity.ToTable("UserAllowedDocTypesSecurityLevels", "dbo");
            entity.HasKey(e => e.ID);
            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER").HasMaxLength(200);
            entity.Property(c => c.AllowedBranchId).HasColumnName("AllowedBranchId").HasColumnType("nvarchar").HasMaxLength(200);

            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").HasColumnType("DateTime");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").HasColumnType("DateTime");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy").HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy").HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");

        }
    }
}
