using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            entity.ToTable("Notification", "dbo");
            entity.HasKey(pk => pk.ID);
            entity.Property(c => c.ID).HasColumnName("ID");
            entity.Property(c => c.TrackingId).HasColumnName("TrackingId").HasColumnType("int").IsRequired(false).HasDefaultValue(0);
            entity.Property(c => c.TrackingNumber).HasColumnName("TrackingNumber").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(false).HasDefaultValue("");

            entity.Property(c => c.FromUserId).HasColumnName("FromUserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne<ApplicationUser>("FromUser").WithMany().HasForeignKey(r => r.FromUserId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ToUserId).HasColumnName("ToUserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne<ApplicationUser>("ToUser").WithMany().HasForeignKey(r => r.ToUserId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.IsRead).HasColumnName("IsRead").HasColumnType("bit").IsRequired(true);

            entity.HasOne<Applications>("Application").WithMany().HasForeignKey(r => r.ApplicationId).OnDelete(DeleteBehavior.NoAction);
             entity.Property(c => c.ApplicationId).HasColumnName("ApplicationId").HasColumnType("int").IsRequired(true);
           
            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
            //entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(r => r.BranchId).OnDelete(DeleteBehavior.NoAction);
          
            entity.Property(c => c.NotificationMessageId).HasColumnName("NotificationMessageId").HasColumnType("int").IsRequired(true);
            entity.HasOne<NotificationMessage>("NotificationMessage").WithMany().HasForeignKey(r => r.NotificationMessageId).OnDelete(DeleteBehavior.NoAction);


            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
       
        }
    }
}