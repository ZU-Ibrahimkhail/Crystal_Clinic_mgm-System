using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> entity)
        {
            entity.ToTable("Permission", "dbo");
            entity.HasKey(pk => pk.Id);
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            entity.Property(c => c.Controller).HasColumnName("Controller").HasColumnType("varchar").HasMaxLength(100);
            entity.Property(c => c.Action).HasColumnName("Action").HasColumnType("nvarchar").HasMaxLength(100);
            entity.Property(c => c.Method).HasColumnName("Method").HasColumnType("nvarchar").HasMaxLength(10);
            entity.Property(c => c.ActionCategory).HasColumnName("ActionCategory").HasColumnType("nvarchar").HasMaxLength(100);
            entity.Property(c => c.ApplicationId).HasColumnName("ApplicationId").HasColumnType("int");
            entity.HasOne<Applications>("Application").WithMany().HasForeignKey(r => r.ApplicationId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.IsGlobal).HasColumnName("IsGlobal").HasColumnType("bit");
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);


            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");

        }
    }
}
