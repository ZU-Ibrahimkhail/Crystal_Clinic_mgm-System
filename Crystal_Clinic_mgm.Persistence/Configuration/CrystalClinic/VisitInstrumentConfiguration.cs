using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class VisitInstrumentConfiguration : IEntityTypeConfiguration<VisitInstrument>
    {
        public void Configure(EntityTypeBuilder<VisitInstrument> entity)
        {
            // Table & Key
            entity.ToTable(nameof(VisitInstrument), "CrystalClinic");
            entity.HasKey(e => e.Id);

            // Visit FK (required) - cascade when visit deleted
            entity.Property(e => e.VisitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();
            entity.HasOne(e => e.Visit)
                .WithMany()
                .HasForeignKey(e => e.VisitId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional InventoryKit
            entity.Property(e => e.KitId)
                .HasColumnName("KitId")
                .HasColumnType("int")
                .IsRequired(false);
            entity.HasOne(e => e.InventoryKit)
                .WithMany()
                .HasForeignKey(e => e.KitId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional ServiceSessions
            entity.Property(e => e.ServiceSessionsId)
                .HasColumnName("ServiceSessionsId")
                .HasColumnType("int")
                .IsRequired(false);
            entity.HasOne(e => e.ServiceSessions)
                .WithMany()
                .HasForeignKey(e => e.ServiceSessionsId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional Item
            entity.Property(e => e.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired(false);
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional InventoryReservation
            entity.Property(e => e.InventoryReservationId)
                .HasColumnName("InventoryReservationId")
                .HasColumnType("int")
                .IsRequired(false);
            entity.HasOne(e => e.InventoryReservation)
                .WithMany()
                .HasForeignKey(e => e.InventoryReservationId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(e => e.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m)
                .IsRequired();

            entity.Property(e => e.DoctorFee)
               .HasColumnName("DoctorFee")
               .HasColumnType("decimal(18,2)")
               .HasDefaultValue(0m)
               .IsRequired();


            entity.Property(e => e.IsFreeForPatient)
                .HasColumnName("IsFreeForPatient")
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .IsRequired();

            entity.Property(e => e.Count)
                .HasColumnName("Count")
                .HasColumnType("int")
                .IsRequired();

            entity.HasIndex(e => new { e.VisitId, e.KitId })
                .IsUnique()
                .HasFilter("[KitId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_KitId");

            entity.HasIndex(e => new { e.VisitId, e.ServiceSessionsId })
                .IsUnique()
                .HasFilter("[ServiceSessionsId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_ServiceSessionsId");

            entity.HasIndex(e => new { e.VisitId, e.ItemId })
                .IsUnique()
                .HasFilter("[ItemId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_ItemId");

            entity.HasIndex(e => new { e.VisitId, e.InventoryReservationId })
                .IsUnique()
                .HasFilter("[InventoryReservationId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_InventoryReservationId");

            // Auditable fields (IsDeleted, CreatedBy, CreatedOn, etc.)
            EntityConfiguration<VisitInstrument>.AuditableEntityConfigurations(entity);
        }
    }
}
