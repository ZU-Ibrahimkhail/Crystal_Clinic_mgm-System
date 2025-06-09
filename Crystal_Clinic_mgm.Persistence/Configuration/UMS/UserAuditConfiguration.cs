using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class UserAuditConfiguration : IEntityTypeConfiguration<UserAudit>
    {
        public void Configure(EntityTypeBuilder<UserAudit> entity)
        {
            entity.ToTable("UserAudit", "dbo");

            entity.HasKey(pk => pk.Id);
 
            entity.Property(c => c.UserName).HasColumnName("UserName").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
 
            entity.Property(c => c.Action).HasColumnName("Action").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.ActionOn).HasColumnName("ActionOn").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.ActionEnd).HasColumnName("ActionEnd").HasColumnType("DateTime").IsRequired(false);
            entity.Property(c => c.IpAddress).HasColumnName("IpAddress").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.DeviceName).HasColumnName("DeviceName").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.Result).HasColumnName("Result").HasColumnType("bit");
            entity.Property(c => c.Message).HasColumnName("Message").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.BrowserName).HasColumnName("BrowserName").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.BrowserVersion).HasColumnName("BrowserVersion").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.Os).HasColumnName("Os").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.DeviceType).HasColumnName("DeviceType").HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER");
            entity.HasOne<ApplicationUser>("User").WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);

            //entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
           // entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(r => r.BranchId).OnDelete(DeleteBehavior.NoAction);




        }
    }
}
