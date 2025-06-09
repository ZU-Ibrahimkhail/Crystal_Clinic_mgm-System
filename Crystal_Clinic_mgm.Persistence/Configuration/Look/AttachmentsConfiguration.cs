using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class AttachmentsConfiguration : IEntityTypeConfiguration<Attachments>
    {
        public void Configure(EntityTypeBuilder<Attachments> entity)
        {
            entity.ToTable("Attachments", "General");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("Id");
            entity.Property(c => c.FilePath).IsRequired(true).HasColumnName("FilePath").HasColumnType("nvarchar").HasMaxLength(500);
            entity.Property(c => c.FileExtention).IsRequired(true).HasColumnName("Comments").HasColumnType("nvarchar").HasMaxLength(50);
            entity.Property(c => c.AttachmentDescription).IsRequired(false).HasColumnName("AttachmentDescription").HasColumnType("nvarchar").HasMaxLength(500);

            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");

        }
    }
}
