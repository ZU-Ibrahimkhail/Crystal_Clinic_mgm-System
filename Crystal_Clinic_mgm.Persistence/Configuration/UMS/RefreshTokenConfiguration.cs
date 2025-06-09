using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.UMS
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity.ToTable("RefreshToken", "dbo");
            entity.HasKey(pk => pk.Id);
            entity.Property(c => c.Id).HasColumnName("Id");

            entity.Property(c => c.Token).HasColumnName("Token").IsRequired(true).HasColumnType("nvarchar").HasMaxLength(1000);
            entity.Property(c => c.Expires).HasColumnName("Expires").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.IsExpired).HasColumnName("IsExpired").IsRequired(true).HasColumnType("bit");
            entity.Property(c => c.Created).HasColumnName("Created").IsRequired(true).HasColumnType("DateTime");
            entity.Property(c => c.CreatedByIp).HasColumnName("CreatedByIp").IsRequired(true).HasColumnType("varchar").HasMaxLength(50);
            entity.Property(c => c.Revoked).HasColumnName("Revoked").IsRequired(false).HasColumnType("DateTime");
            entity.Property(c => c.RevokedByIp).HasColumnName("RevokedByIp").IsRequired(false).HasColumnType("varchar").HasMaxLength(50);
            entity.Property(c => c.ReplacedByToken).HasColumnName("ReplacedByToken").IsRequired(false).HasColumnType("nvarchar").HasMaxLength(1000);
            entity.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired(true).HasColumnType("bit");

        }
    }
}
