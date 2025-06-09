using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class EmailHistoryConfiguration : IEntityTypeConfiguration<EmailHistory>
    {
        public void Configure(EntityTypeBuilder<EmailHistory> entity)
        {
            entity.ToTable("EmailHistory", "dbo");

            entity.HasKey(pk => pk.ID);
            entity.Property(c => c.ID).HasColumnName("ID").HasColumnType("int");
            entity.Property(c => c.Subject).HasColumnName("Subject").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.Body).HasColumnName("Body").HasColumnType("nvarchar").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.ToEmail).HasColumnName("ToEmail").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.IsVerified).HasColumnName("IsVerified").HasColumnType("bit").HasDefaultValue(false);


            entity.Property(c => c.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(50).IsRequired(true).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(50).IsRequired(false).HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.ModifiedOn).HasColumnName("ModifiedOn").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");
            entity.Property(c => c.Remarks).HasColumnName("Remarks").IsRequired(false).HasMaxLength(500).HasColumnType("nvarchar");
        }
    }
}
