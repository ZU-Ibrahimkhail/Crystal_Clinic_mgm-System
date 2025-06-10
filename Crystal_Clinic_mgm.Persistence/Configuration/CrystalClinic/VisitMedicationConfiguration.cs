using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class VisitMedicationConfiguration : IEntityTypeConfiguration<VisitMedication>
    {
        public void Configure(EntityTypeBuilder<VisitMedication> entity)
        {
            // Table Name
            entity.ToTable(nameof(VisitMedication), "CrystalClinic");

            // Primary Key
            entity.HasKey(vm => vm.medicationId);

            // Foreign Key relationships
            entity.HasOne(vm => vm.stock)
                .WithMany()
                .HasForeignKey(vm => vm.stockId)
                .OnDelete(DeleteBehavior.Restrict);  // Set appropriate delete behavior for stock

            // Properties configurations
            entity.Property(vm => vm.name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(255)")
                .IsRequired();

            entity.Property(vm => vm.dosage)
                .HasColumnName("Dosage")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            entity.Property(vm => vm.quantity)
                .HasColumnName("Quantity")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(vm => vm.price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            // Optional: You could also store additional properties like creation and update timestamps (AuditableEntity)
            EntityConfiguration<VisitMedication>.AuditableEntityConfigurations(entity);
        }
    }
}
