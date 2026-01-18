using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class SalesEstimateConfiguration : IEntityTypeConfiguration<SalesEstimate>
    {
        public void Configure(EntityTypeBuilder<SalesEstimate> entity)
        {
            entity.ToTable(nameof(SalesEstimate), "Accounting");

            entity.HasKey(se => se.Id);

            entity.Property(se => se.EstimateNumber)
                .HasColumnName("EstimateNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            entity.Property(se => se.PatientId)
                .HasColumnName("PatientId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(se => se.EstimateDate)
                .HasColumnName("EstimateDate")
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(se => se.ValidUntil)
                .HasColumnName("ValidUntil")
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(se => se.Subtotal)
                .HasColumnName("Subtotal")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(se => se.TaxAmount)
                .HasColumnName("TaxAmount")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(se => se.DiscountAmount)
                .HasColumnName("DiscountAmount")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(se => se.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(se => se.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(se => se.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(se => se.ConvertedToInvoiceId)
                .HasColumnName("ConvertedToInvoiceId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(se => se.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(se => se.Patient)
                .WithMany()
                .HasForeignKey(se => se.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(se => se.ConvertedToInvoice)
                .WithMany()
                .HasForeignKey(se => se.ConvertedToInvoiceId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(se => se.EstimateLines)
                .WithOne(sel => sel.SalesEstimate)
                .HasForeignKey(sel => sel.SalesEstimateId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<SalesEstimate>.AuditableEntityConfigurations(entity);
        }
    }
}
