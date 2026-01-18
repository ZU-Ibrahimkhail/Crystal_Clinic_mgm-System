using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class InventoryKitLineConfiguration : IEntityTypeConfiguration<InventoryKitLine>
    {
        public void Configure(EntityTypeBuilder<InventoryKitLine> entity)
        {
            entity.ToTable(nameof(InventoryKitLine), "BranchStock");

            entity.HasKey(ikl => ikl.Id);

            entity.Property(ikl => ikl.KitId)
                .HasColumnName("KitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ikl => ikl.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ikl => ikl.Quantity)
                .HasColumnName("Quantity")
                .HasPrecision(18, 4)
                .IsRequired();

            // Foreign Key relationships
            entity.HasOne(ikl => ikl.Kit)
                .WithMany(k => k.KitLines)
                .HasForeignKey(ikl => ikl.KitId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ikl => ikl.Item)
                .WithMany()
                .HasForeignKey(ikl => ikl.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<InventoryKitLine>.AuditableEntityConfigurations(entity);
        }
    }
}
