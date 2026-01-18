using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock.Look
{
    public class ItemUnitConfiguration : IEntityTypeConfiguration<ItemUnit>
    {
        public void Configure(EntityTypeBuilder<ItemUnit> entity)
        {
            entity.ToTable(nameof(ItemUnit), "Stock");

            entity.HasKey(iu => iu.unitId);

            entity.Property(iu => iu.UnitName)
                .HasColumnName("UnitName")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            entity.Property(iu => iu.ConversionFactor)
                .HasColumnName("ConversionFactor")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(iu => iu.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(iu => iu.DailyRentalPrice)
                .HasColumnName("DailyRentalPrice")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(iu => iu.SellingPrice)
                .HasColumnName("SellingPrice")
                .HasColumnType("int")
                .IsRequired();

            entity.HasOne(iu => iu.Item)
                .WithMany()
                .HasForeignKey(iu => iu.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
