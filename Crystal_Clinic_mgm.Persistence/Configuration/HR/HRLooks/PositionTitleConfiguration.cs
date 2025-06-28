using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class PositionTitleConfiguration : IEntityTypeConfiguration<PositionTitle>
    {
        public void Configure(EntityTypeBuilder<PositionTitle> entity)
        {
            entity.ToTable("PositionTitle", "HRLooks");
            entity.HasKey("ID");
            entity.Property(c => c.ID).HasColumnName("ID");
            entity.Property(c => c.EnglishName).HasColumnName("EnglishName").HasMaxLength(300).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.PashtoName).HasColumnName("PashtoName").HasMaxLength(300).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.DariName).HasColumnName("DariName").HasMaxLength(300).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.Code).HasColumnName("Code").IsRequired(true).HasMaxLength(50).HasColumnType("nvarchar");

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true).HasDefaultValue(1);
            entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);
            entity.Property(c => c.JobDescription).HasColumnName("JobDescription").HasColumnType("nvarchar(max)").IsRequired(false);



            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}

