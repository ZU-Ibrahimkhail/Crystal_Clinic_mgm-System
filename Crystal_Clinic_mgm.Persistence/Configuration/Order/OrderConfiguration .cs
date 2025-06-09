// Updated OrderConfiguration.cs
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> entity)
        {
            entity.ToTable(nameof(Orders), "Sales");
            entity.HasKey(x => x.OrderId);

            entity.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
            entity.Property(x => x.OrderDate).HasColumnType("datetime2").IsRequired();
            entity.Property(x => x.ScheduledDate).HasColumnType("datetime2");
            entity.Property(x => x.OrderType).HasColumnType("nvarchar(20)").IsRequired();
            entity.Property(x => x.OriginalTotal).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.AdjustedTotal).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.PaidAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Status).HasColumnType("nvarchar(20)").IsRequired();
            entity.Property(x => x.DeliveryNotes).HasMaxLength(500);

            // Relationships
            entity.HasOne(x => x.Customer)
                  .WithMany(x => x.Orders)
                  .HasForeignKey(x => x.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Employee)
                  .WithMany()
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Branch)
                  .WithMany()
                  .HasForeignKey(x => x.BranchId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(x => x.OrderItems)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId);

            entity.HasMany(x => x.OrderServices)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId);

            entity.HasMany(x => x.Payments)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId);

            entity.HasMany(x => x.Adjustments)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId);
        }
    }
}