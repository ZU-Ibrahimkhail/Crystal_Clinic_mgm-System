using Crystal_Clinic_Mgm.Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.General
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> entity)
        {
            entity.ToTable("News", "General");
            entity.HasKey("ID");
            entity.Property(c => c.Title).HasColumnName("Title").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);         
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(true);         
            entity.Property(c => c.Speaker).HasColumnName("Speaker").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(false);         
            entity.Property(c => c.Location).HasColumnName("Location").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(false);
            entity.Property(c => c.ShowNotification).HasColumnName("ShowNotification").HasColumnType("bit").HasDefaultValue(false);
            entity.Property(c => c.StartTime).HasColumnName("StartTime").HasColumnType("DateTime").IsRequired(false);
            entity.Property(c => c.EndTime).HasColumnName("EndTime").HasColumnType("DateTime").IsRequired(false);
            entity.Property(c => c.NewsDate).HasColumnName("NewsDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.AttachmentPath).HasColumnName("AttachmentPath").HasColumnType("nvarchar").HasMaxLength(700).IsRequired(false);
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}
