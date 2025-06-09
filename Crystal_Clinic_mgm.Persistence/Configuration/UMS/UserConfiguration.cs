using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.ToTable("User", "dbo");

            entity.HasKey(pk => pk.Id);
            entity.Property(c => c.Id).HasColumnName("ID");

            entity.Property(c => c.AccessFailedCount).HasColumnName("FailedLoginCount");
            entity.Property(c => c.PasswordHash).HasColumnName("Password");
            entity.Property(c => c.SuccessLoginCount).HasColumnName("SuccessLoginCount").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.LastLoginDate).HasColumnName("LastLoginDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int");
            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int");
            entity.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired(true);
            entity.Property(c => c.RefreshToken).HasColumnName("RefreshToken").HasColumnType("nvarchar").HasMaxLength(1000).IsRequired(false);
            
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").HasColumnType("DateTime");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").HasColumnType("DateTime");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy").HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy").HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").IsRequired(true).HasColumnType("bit");
            
        }
    }
}
