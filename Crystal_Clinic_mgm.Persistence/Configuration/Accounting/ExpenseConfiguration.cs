using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> entity)
        {
            entity.ToTable(nameof(Expense), "Accounting");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("int")
                .IsRequired();


            entity.Property(e => e.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.Property(e => e.ExpenseDate)
                .HasColumnName("ExpenseDate")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(e => e.IsReimbursable)
                .HasColumnName("IsReimbursable")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.ChartOfAccountId)
                .HasColumnName("ChartOfAccountId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(e => e.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("int")
                .IsRequired(false);

            entity.Property(c => c.AttachmentPath)
                    .HasColumnName("AttachmentPath")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                        )
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.ChartOfAccount)
                .WithMany()
                .HasForeignKey(e => e.ChartOfAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Branch)
                .WithMany()
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            EntityConfiguration<Expense>.AuditableEntityConfigurations(entity);
        }
    }
}
