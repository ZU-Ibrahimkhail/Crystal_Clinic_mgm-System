using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.ToTable("Language", "dbo");
            entity.HasKey(pk => pk.ID);          
            entity.Property(c => c.ID).HasColumnName("ID");
            entity.Property(c => c.EnglishName).HasColumnName("EnglishName").HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.PashtoName).HasColumnName("PashtoName").HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.DariName).HasColumnName("DariName").HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.Code).HasColumnName("Code").IsRequired(false).HasMaxLength(50).HasColumnType("nvarchar");
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}
