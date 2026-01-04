using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ForecastSnapshotConfiguration : IEntityTypeConfiguration<ForecastSnapshot>
    {
        public void Configure(EntityTypeBuilder<ForecastSnapshot> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(f => f.Scenario)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(f => f.ProjectedCashBalance)
                .HasPrecision(18, 4);

            builder.Property(f => f.ProjectedAccountsReceivable)
                .HasPrecision(18, 4);

            builder.Property(f => f.ProjectedAccountsPayable)
                .HasPrecision(18, 4);

            builder.Property(f => f.ProjectedNetIncome)
                .HasPrecision(18, 4);

            builder.Property(f => f.Notes)
                .HasMaxLength(1000);

            builder.HasMany(f => f.Lines)
                .WithOne(l => l.ForecastSnapshot)
                .HasForeignKey(l => l.ForecastSnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => f.Scenario);
            builder.HasIndex(f => f.Status);
            builder.HasIndex(f => f.SnapshotDate);
        }
    }
}
