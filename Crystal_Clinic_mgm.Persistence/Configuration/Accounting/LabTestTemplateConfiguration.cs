using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class LabTestTemplateConfiguration : IEntityTypeConfiguration<LabTestTemplate>
    {
        public void Configure(EntityTypeBuilder<LabTestTemplate> entity)
        {
            entity.ToTable(nameof(LabTestTemplate), "Accounting");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.TestName)
                .HasColumnName("TestName")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(l => l.Category)
                .HasColumnName("Category")
                .HasColumnType("nvarchar(255)")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(l => l.NormalRangeMin)
                .HasColumnName("NormalRangeMin")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(l => l.NormalRangeMax)
                .HasColumnName("NormalRangeMax")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(l => l.Unit)
                .HasColumnName("Unit")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(l => l.ExpectedResult)
                .HasColumnName("ExpectedResult")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(l => l.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(l => l.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.HasMany(l => l.OrderLines)
                .WithOne(o => o.Template)
                .HasForeignKey(o => o.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            EntityConfiguration<LabTestTemplate>.AuditableEntityConfigurations(entity);
        }
    }
}
