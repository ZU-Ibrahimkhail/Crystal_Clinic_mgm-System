using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class TrackingTableConfiguration : IEntityTypeConfiguration<TrackingTable>
    {
        public void Configure(EntityTypeBuilder<TrackingTable> entity)
        {

            entity.ToTable("TrackingTable", "dbo");
            entity.HasKey(pk => pk.Id);
            entity.Property(c => c.Id).HasColumnName("ID");
            entity.Property(c => c.UserAuditId).HasColumnName("UserAuditId").HasColumnType("int").IsRequired(false);
            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.Property(c => c.ApplicationId).HasColumnName("ApplicationId").HasColumnType("int").IsRequired(false);
            entity.Property(c => c.PermissionId).HasColumnName("PermissionId").HasColumnType("int").IsRequired(false);
            entity.Property(c => c.IsAccessed).HasColumnName("IsAccessed").HasColumnType("bit").IsRequired(true);
            entity.Property(c => c.RequestTime).HasColumnName("RequestTime").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.ErrorDetails).HasColumnName("ErrorDetails").HasColumnType("varchar").HasMaxLength(250);
            entity.Property(c => c.ActionName).HasColumnName("ActionName").HasColumnType("varchar").HasMaxLength(250);
            entity.Property(c => c.Url).HasColumnName("Url").HasColumnType("varchar").HasMaxLength(250);
            entity.HasOne<ApplicationUser>("User").WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<UserAudit>("UserAudit").WithMany().HasForeignKey(c => c.UserAuditId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
