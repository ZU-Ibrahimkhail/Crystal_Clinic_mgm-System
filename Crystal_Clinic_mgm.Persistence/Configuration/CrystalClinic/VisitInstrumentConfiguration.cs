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
            entity.Property(e => e.VisitKitsId)
                .HasColumnName("VisitKitsId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(e => e.VisitKits)
                .WithMany()
                .HasForeignKey(e => e.VisitKitsId)
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

            // Optional Medication
            entity.Property(e => e.VisitMedicationId)
                .HasColumnName("VisitMedicationId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(e => e.VisitMedication)
                .WithMany()
                .HasForeignKey(e => e.VisitMedicationId)
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

            entity.HasIndex(e => new { e.VisitId, e.VisitKitsId })
                .IsUnique()
                .HasFilter("[VisitKitsId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitKitsId_KitId");

            entity.HasIndex(e => new { e.VisitId, e.ServiceSessionsId })
                .IsUnique()
                .HasFilter("[ServiceSessionsId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_ServiceSessionsId");

            entity.HasIndex(e => new { e.VisitId, e.VisitMedicationId })
                .IsUnique()
                .HasFilter("[VisitMedicationId] IS NOT NULL")
                .HasDatabaseName("UQ_VisitInstrument_VisitId_VisitMedicationId");

            EntityConfiguration<VisitInstrument>.AuditableEntityConfigurations(entity);
        }
    }
}
