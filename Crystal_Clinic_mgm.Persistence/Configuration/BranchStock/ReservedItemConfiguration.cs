using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class ReservedItemConfiguration : IEntityTypeConfiguration<ReservedItem>
    {
        public void Configure(EntityTypeBuilder<ReservedItem> entity)
        {
            entity.ToTable(nameof(ReservedItem), "BranchStock");

            entity.HasKey(ri => ri.Id);

            entity.Property(ri => ri.ReservationId)
                .HasColumnName("ReservationId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ri => ri.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ri => ri.StockId)
                .HasColumnName("StockId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ri => ri.ReservedQuantity)
                .HasColumnName("ReservedQuantity")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(ri => ri.UnitCost)
                .HasColumnName("UnitCost")
                .HasPrecision(18, 4)
                .IsRequired();

            // Foreign Key relationships
            entity.HasOne(ri => ri.Reservation)
                .WithMany(r => r.ReservedItems)
                .HasForeignKey(ri => ri.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ri => ri.Item)
                .WithMany()
                .HasForeignKey(ri => ri.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ri => ri.Stock)
                .WithMany()
                .HasForeignKey(ri => ri.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<ReservedItem>.AuditableEntityConfigurations(entity);
        }
    }
}
