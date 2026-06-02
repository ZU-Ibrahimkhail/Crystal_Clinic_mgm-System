using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class POLineConfiguration : IEntityTypeConfiguration<POLine>
    {
        public void Configure(EntityTypeBuilder<POLine> entity)
        {
            entity.ToTable(nameof(POLine), "Accounting");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.PurchaseOrderId)
                .HasColumnName("PurchaseOrderId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.ItemDescription)
                .HasColumnName("ItemDescription")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(p => p.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(p => p.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(p => p.LineTotal)
                .HasColumnName("LineTotal")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(c => c.BarCode)
                .HasColumnName("BarCode")
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            entity.Property(c => c.BatchNumber)
                .HasColumnName("BatchNumber")
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            entity.Property(p => p.ReceivedQuantity)
                .HasColumnName("ReceivedQuantity")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(p => p.PurchaseOrder)
                .WithMany(o => o.Lines)
                .HasForeignKey(p => p.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<POLine>.AuditableEntityConfigurations(entity);
        }
    }
}
