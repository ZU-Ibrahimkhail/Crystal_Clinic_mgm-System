using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class FixedAssetConfiguration : IEntityTypeConfiguration<FixedAsset>
    {
        public void Configure(EntityTypeBuilder<FixedAsset> entity)
        {
            entity.ToTable(nameof(FixedAsset), "Accounting");

            entity.HasKey(f => f.Id);

            entity.Property(f => f.AssetCode)
                .HasColumnName("AssetCode")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(f => f.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(f => f.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(f => f.PurchaseValue)
                .HasColumnName("PurchaseValue")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(f => f.ResidualValue)
                .HasColumnName("ResidualValue")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(f => f.UsefulLifeMonths)
                .HasColumnName("UsefulLifeMonths")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(f => f.AcquisitionDate)
                .HasColumnName("AcquisitionDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(f => f.AccumulatedDepreciation)
                .HasColumnName("AccumulatedDepreciation")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(f => f.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(f => f.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(f => f.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(f => f.SerialNumber)
                .HasColumnName("SerialNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired(false)
                .HasMaxLength(100);

            entity.HasOne(f => f.Branch)
                .WithMany()
                .HasForeignKey(f => f.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(f => f.AssetCode).IsUnique();

            EntityConfiguration<FixedAsset>.AuditableEntityConfigurations(entity);
        }
    }
}
