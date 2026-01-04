using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
    {
        public void Configure(EntityTypeBuilder<SalesInvoice> entity)
        {
            entity.ToTable(nameof(SalesInvoice), "Accounting");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.InvoiceNumber)
                .HasColumnName("InvoiceNumber")
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(s => s.InvoiceDate)
                .HasColumnName("InvoiceDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(s => s.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.TaxAmount)
                .HasColumnName("TaxAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(s => s.DiscountAmount)
                .HasColumnName("DiscountAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(s => s.NetAmount)
                .HasColumnName("NetAmount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(s => s.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(SalesStatus.Draft);

            entity.Property(s => s.SalesArea)
                .HasColumnName("SalesArea")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(s => s.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Branch)
                .WithMany()
                .HasForeignKey(s => s.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(s => s.Lines)
                .WithOne(l => l.SalesInvoice)
                .HasForeignKey(l => l.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.Receipts)
                .WithOne(r => r.SalesInvoice)
                .HasForeignKey(r => r.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(s => s.InvoiceNumber).IsUnique();

            EntityConfiguration<SalesInvoice>.AuditableEntityConfigurations(entity);
        }
    }
}
