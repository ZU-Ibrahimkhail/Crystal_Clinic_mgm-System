using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> entity)
        {
            // Table Name
            entity.ToTable(nameof(Visit), "CrystalClinic");

            // Primary Key
            entity.HasKey(v => v.visitId);

            // Foreign Key relationships
            entity.HasOne(v => v.Patient)
                .WithMany()
                .HasForeignKey(v => v.patientId)
                .OnDelete(DeleteBehavior.Restrict);  // Define the delete behavior based on your requirements

            entity.HasOne(v => v.Doctor)
                .WithMany()
                .HasForeignKey(v => v.doctorId)
                .OnDelete(DeleteBehavior.SetNull);  // If no doctor is assigned, set to null
            
            entity.Property(v => v.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired().HasDefaultValue(1);
            
            entity.Property(v => v.BranchDetailsId)
                .HasColumnName("BranchDetailsId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(v => v.BranchDetails)
                .WithMany()
                .HasForeignKey(v => v.BranchDetailsId)
                .OnDelete(DeleteBehavior.SetNull);  // If no doctor is assigned, set to null

            // Properties configurations
            entity.Property(v => v.visitDate)
                .HasColumnName("VisitDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(v => v.status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(v => v.FeeAmount)
                .HasColumnName("FeeAmount")
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValue(0)
                .IsRequired();     
            
            entity.Property(v => v.totalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(v => v.paidAmount)
                .HasColumnName("PaidAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(v => v.remainingAmount)
                .HasColumnName("RemainingAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            // Collection Properties (VisitMedications, Services)
            entity.HasMany(v => v.Medications)
                .WithOne()
                .HasForeignKey(vm => vm.visitId)  // Assuming a foreign key of visitId in VisitMedication
                .OnDelete(DeleteBehavior.Cascade);
            // Collection Properties (VisitMedications, Services)
            entity.HasMany(v => v.Payments)
                .WithOne()
                .HasForeignKey(vm => vm.visitId)  // Assuming a foreign key of visitId in VisitMedication
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(v => v.Services)
                .WithOne()
                .HasForeignKey(s => s.visitId)  // Assuming a foreign key of visitId in Service
                .OnDelete(DeleteBehavior.Cascade);


            // Auditable entity fields configuration (if applicable)
            EntityConfiguration<Visit>.AuditableEntityConfigurations(entity);
        }
    }
}
