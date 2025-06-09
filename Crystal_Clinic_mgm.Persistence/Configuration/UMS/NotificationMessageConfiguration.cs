using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class NotificationMessageConfiguration : IEntityTypeConfiguration<NotificationMessage>
    {
        public void Configure(EntityTypeBuilder<NotificationMessage> entity)
        {
            entity.ToTable("NotificationMessage", "dbo");
            entity.HasKey(pk => pk.ID);
            entity.Property(c => c.ID).HasColumnName("ID");

            entity.Property(c => c.EnglishTitle).HasColumnName("EnglishTitle").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.DariTitle).HasColumnName("DariTitle").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.PashtoTitle).HasColumnName("PashtoTitle").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.EnglishMessage).HasColumnName("EnglishMessage").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.DariMessage).HasColumnName("DariMessage").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.PashtoMessage).HasColumnName("PashtoMessage").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.ApplicationName).HasColumnName("ApplicationName").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.EnglishDescription).HasColumnName("EnglishDescription").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.DariDescription).HasColumnName("DariDescription").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.PashtoDescription).HasColumnName("PashtoDescription").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.ControllerLink).HasColumnName("ControllerLink").IsRequired(false).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");
            entity.Property(c => c.ActionLink).HasColumnName("ActionLink").IsRequired(false).HasMaxLength(50).HasColumnType("nvarchar").HasDefaultValue("");

            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");

        }
    }
}