using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("UserRole", "dbo");

            entity.Property(c => c.ID).HasColumnName("ID").ValueGeneratedOnAdd();
            entity.Property(c => c.RoleId).HasColumnName("RoleID");
            entity.Property(c => c.UserId).HasColumnName("UserID").HasColumnType("UNIQUEIDENTIFIER");
            entity.HasOne<ApplicationRole>("Role").WithMany().HasForeignKey(r => r.RoleId);
            entity.HasOne<ApplicationUser>("User").WithMany().HasForeignKey(r => r.UserId);
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").IsRequired(true).HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");


        }
    }
}
