using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> entity)
        {
            entity.ToTable("Role", "dbo");
            entity.Property(c => c.RoleDescription).HasColumnName("RoleDescription").HasColumnType("nvarchar").HasMaxLength(250);


            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
        
            entity.HasOne<Applications>("Application").WithMany().HasForeignKey(r => r.ApplicationId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
