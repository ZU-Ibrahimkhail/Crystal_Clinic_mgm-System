using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    internal class InventoryReservationConfiguration : IEntityTypeConfiguration<InventoryReservation>
    {
        public void Configure(EntityTypeBuilder<InventoryReservation> entity)
        {
            entity.ToTable(nameof(InventoryReservation), "InventoryReservation");

            entity.HasKey(ir => ir.Id);

            entity.Property(ir => ir.VisitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ir => ir.ServiceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(ir => ir.IdempotencyToken)
                .HasColumnName("IdempotencyToken")
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            entity.Property(ir => ir.ExpiresAt)
                .HasColumnName("ExpiresAt")
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(ir => ir.RequestedBy)
                .HasColumnName("RequestedBy")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            entity.Property(ir => ir.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(ir => ir.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(ir => ir.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion()
                .IsConcurrencyToken();

            // Foreign Key relationships
            entity.HasOne(ir => ir.Visit)
                .WithMany()
                .HasForeignKey(ir => ir.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ir => ir.Service)
                .WithMany()
                .HasForeignKey(ir => ir.ServiceId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(ir => ir.ReservedItems)
                .WithOne()
                .HasForeignKey("InventoryReservationId")
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<InventoryReservation>
                .AuditableEntityConfigurations(entity);
        }
    }
}
