using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.General
{
    public class NewsNotificationConfiguration : IEntityTypeConfiguration<NewsNotification>
    {
        public void Configure(EntityTypeBuilder<NewsNotification> entity)
        {
            entity.ToTable("NewsNotification", "General");
            entity.HasKey("ID");
            entity.Property(c => c.ID).HasColumnName("ID");
            entity.Property(c => c.NewsId).HasColumnName("NewsId").HasColumnType("int").IsRequired(true).HasDefaultValue(1);
            entity.HasOne<News>("News").WithMany().HasForeignKey(f => f.NewsId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
            entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}

