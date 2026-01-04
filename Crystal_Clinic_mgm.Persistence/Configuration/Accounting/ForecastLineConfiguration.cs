using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ForecastLineConfiguration : IEntityTypeConfiguration<ForecastLine>
    {
        public void Configure(EntityTypeBuilder<ForecastLine> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.MetricType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(l => l.MetricName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(l => l.ProjectedValue)
                .HasPrecision(18, 4);

            builder.Property(l => l.VariancePercentage)
                .HasPrecision(10, 2);

            builder.Property(l => l.Notes)
                .HasMaxLength(500);

            builder.HasOne(l => l.ForecastSnapshot)
                .WithMany(f => f.Lines)
                .HasForeignKey(l => l.ForecastSnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.ForecastSnapshotId);
            builder.HasIndex(l => l.ForecastMonth);
            builder.HasIndex(l => l.MetricType);
        }
    }
}
