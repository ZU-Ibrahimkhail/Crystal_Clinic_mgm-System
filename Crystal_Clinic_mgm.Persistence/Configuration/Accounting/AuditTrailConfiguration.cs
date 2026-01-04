using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.EntityType)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Action)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.UserName)
                .HasMaxLength(255);

            builder.Property(a => a.BeforeValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.AfterValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.CorrelationId)
                .HasMaxLength(100);

            builder.Property(a => a.IpAddress)
                .HasMaxLength(50);

            builder.Property(a => a.UserAgent)
                .HasMaxLength(500);

            builder.Property(a => a.RelatedEntityType)
                .HasMaxLength(255);

            builder.HasIndex(a => a.EntityType);
            builder.HasIndex(a => a.EntityId);
            builder.HasIndex(a => a.Action);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.AuditDate);
            builder.HasIndex(a => a.CorrelationId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
        }
    }
}
