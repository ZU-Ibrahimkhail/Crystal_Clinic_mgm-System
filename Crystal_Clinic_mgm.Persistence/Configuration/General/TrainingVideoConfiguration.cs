using Crystal_Clinic_Mgm.Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.General
{
    public class TrainingVideoConfiguration : IEntityTypeConfiguration<TrainingVideo>
    {
        public void Configure(EntityTypeBuilder<TrainingVideo> entity)
        {
            entity.ToTable("TrainingVideos", "General");
            entity.HasKey("Id");
            entity.Property(c => c.Id).HasColumnName("Id");
            entity.Property(c => c.DariTitle).HasColumnName("DariTitle").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
            entity.Property(c => c.PashtoTitle).HasColumnName("PashtoTitle").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
            entity.Property(c => c.Application).HasColumnName("Application").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);

            entity.Property(c => c.Poster).HasColumnName("Poster").HasColumnType("nvarchar(max)").IsRequired(true);
            entity.Property(c => c.DariVideoPath).HasColumnName("DariVideoPath").HasColumnType("nvarchar(max)").IsRequired(true);
            entity.Property(c => c.PashtoVideoPath).HasColumnName("PashtoVideoPath").HasColumnType("nvarchar(max)").IsRequired(true);

            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}
