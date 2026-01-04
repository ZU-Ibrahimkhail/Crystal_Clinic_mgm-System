using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class SalesInvoiceLineConfiguration : IEntityTypeConfiguration<SalesInvoiceLine>
    {
        public void Configure(EntityTypeBuilder<SalesInvoiceLine> entity)
        {
            entity.ToTable(nameof(SalesInvoiceLine), "Accounting");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.SalesInvoiceId)
                .HasColumnName("SalesInvoiceId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.ServiceId)
                .HasColumnName("ServiceId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(s => s.InventoryItemId)
                .HasColumnName("InventoryItemId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(s => s.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(s => s.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.LineTotal)
                .HasColumnName("LineTotal")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.DiscountAmount)
                .HasColumnName("DiscountAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(s => s.TaxAmount)
                .HasColumnName("TaxAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(s => s.SalesInvoice)
                .WithMany(i => i.Lines)
                .HasForeignKey(s => s.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            EntityConfiguration<SalesInvoiceLine>.AuditableEntityConfigurations(entity);
        }
    }
}
