using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS
{
    public class ExpenseTrackingConfiguration : IEntityTypeConfiguration<ExpenseTracking>
    {
        public void Configure(EntityTypeBuilder<ExpenseTracking> entity)
        {
            entity.ToTable(nameof(ExpenseTracking), "AssetMS");
            entity.HasKey("ID");
            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(x => x.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ExpenseTypeId).HasColumnName("ExpenseTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.ExpenseType).WithMany().HasForeignKey(x => x.ExpenseTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.MainAccountId).HasColumnName("MainAccountId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne(x => x.MainAccount).WithMany().HasForeignKey(x => x.MainAccountId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.Amount).HasColumnName("Amount").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.Date).HasColumnName("Date").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.InvoiceNumber).HasColumnName("InvoiceNumber").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            entity.Property(c => c.AttachmentPath)
                    .HasColumnName("AttachmentPath")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                        )
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);

            EntityConfiguration<ExpenseTracking>.AuditableEntityConfigurations(entity);

        }
    }
}
