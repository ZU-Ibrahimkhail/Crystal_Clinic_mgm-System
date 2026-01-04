using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ProcedureLogConfiguration : IEntityTypeConfiguration<ProcedureLog>
    {
        public void Configure(EntityTypeBuilder<ProcedureLog> entity)
        {
            entity.ToTable(nameof(ProcedureLog), "Accounting");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.VisitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.FixedAssetId)
                .HasColumnName("FixedAssetId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.RoomId)
                .HasColumnName("RoomId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(p => p.StartTime)
                .HasColumnName("StartTime")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(p => p.EndTime)
                .HasColumnName("EndTime")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(p => p.DurationMinutes)
                .HasColumnName("DurationMinutes")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(p => p.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.HasOne(p => p.Visit)
                .WithMany()
                .HasForeignKey(p => p.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.FixedAsset)
                .WithMany()
                .HasForeignKey(p => p.FixedAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<ProcedureLog>.AuditableEntityConfigurations(entity);
        }
    }
}
