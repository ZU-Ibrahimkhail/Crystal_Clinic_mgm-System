using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class SalesEstimateLineConfiguration : IEntityTypeConfiguration<SalesEstimateLine>
    {
        public void Configure(EntityTypeBuilder<SalesEstimateLine> entity)
        {
            entity.ToTable(nameof(SalesEstimateLine), "Accounting");

            entity.HasKey(sel => sel.Id);

            entity.Property(sel => sel.SalesEstimateId)
                .HasColumnName("SalesEstimateId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(sel => sel.ServiceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(sel => sel.Quantity)
                .HasColumnName("Quantity")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(sel => sel.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(sel => sel.TotalPrice)
                .HasColumnName("TotalPrice")
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(sel => sel.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(sel => sel.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(sel => sel.SalesEstimate)
                .WithMany(se => se.EstimateLines)
                .HasForeignKey(sel => sel.SalesEstimateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sel => sel.Service)
                .WithMany()
                .HasForeignKey(sel => sel.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sel => sel.Item)
                .WithMany()
                .HasForeignKey(sel => sel.ItemId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<SalesEstimateLine>.AuditableEntityConfigurations(entity);
        }
    }
}
