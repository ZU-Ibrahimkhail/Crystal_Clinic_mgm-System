using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class LabOrderLineConfiguration : IEntityTypeConfiguration<LabOrderLine>
    {
        public void Configure(EntityTypeBuilder<LabOrderLine> entity)
        {
            entity.ToTable(nameof(LabOrderLine), "Accounting");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.VisitId)
                .HasColumnName("VisitId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(l => l.TemplateId)
                .HasColumnName("TemplateId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(l => l.ActualValue)
                .HasColumnName("ActualValue")
                .HasColumnType("decimal(18, 2)")
                .IsRequired(false);

            entity.Property(l => l.ActualResult)
                .HasColumnName("ActualResult")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(l => l.IsAbnormal)
                .HasColumnName("IsAbnormal")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(l => l.OrderDate)
                .HasColumnName("OrderDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(l => l.ResultDate)
                .HasColumnName("ResultDate")
                .HasColumnType("datetime")
                .IsRequired(false);

            entity.Property(l => l.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.HasOne(l => l.Visit)
                .WithMany()
                .HasForeignKey(l => l.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Template)
                .WithMany(t => t.OrderLines)
                .HasForeignKey(l => l.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<LabOrderLine>.AuditableEntityConfigurations(entity);
        }
    }
}
