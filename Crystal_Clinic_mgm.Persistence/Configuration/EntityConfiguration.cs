using Crystal_Clinic_Mgm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration
{
    public class EntityConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public static EntityTypeBuilder<TEntity> AuditableEntityConfigurations(EntityTypeBuilder<TEntity> entity)
        {
            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
            return entity;
        }
    }
    public class EntityConfigurations<TLookEntity> where TLookEntity : LookAndAuditableEntity
    {
        public static EntityTypeBuilder<TLookEntity> LookAuditableEntityConfigurations(EntityTypeBuilder<TLookEntity> entity)
        {

            entity.HasKey("ID");
            entity.Property(c => c.ID).HasColumnName("ID");
            //----EnglishName
            entity.Property(p => p.EnglishName).HasColumnName("EnglishName").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            //----PashtoName
            entity.Property(p => p.PashtoName).HasColumnName("PashtoName").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            //----DariName
            entity.Property(p => p.DariName).HasColumnName("DariName").HasColumnType("nvarchar").HasMaxLength(70).IsRequired(true);
            //----Code
            entity.Property(p => p.Code).HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            EntityConfiguration<TLookEntity>.AuditableEntityConfigurations(entity);
            return entity;
        }

    }
}
