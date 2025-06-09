// Remaining Entity Configurations
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.ToTable(nameof(Customer), "Sales");
            entity.HasKey(x => x.CustomerId);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Phone).HasMaxLength(20);
            entity.Property(x => x.Email).HasMaxLength(100);
        }
    }

    public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> entity)
        {
            entity.ToTable(nameof(ItemCategory), "Inventory");
            entity.HasKey(x => x.categoryId);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
        }
    }

    public class ItemUnitConfiguration : IEntityTypeConfiguration<ItemUnit>
    {
        public void Configure(EntityTypeBuilder<ItemUnit> entity)
        {
            entity.ToTable(nameof(ItemUnit), "Inventory");
            entity.HasKey(x => x.unitId);
            entity.Property(x => x.UnitName).IsRequired().HasMaxLength(50);
            entity.Property(x => x.ConversionFactor).HasColumnType("decimal(10,2)");
        }
    }
    
    public class ReturnPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
    {
        public void Configure(EntityTypeBuilder<OrderPayment> entity)
        {
            entity.ToTable(nameof(OrderPayment), "Payments");
            entity.HasKey(x => x.PaymentId);
            entity.Property(x => x.PaymentId).HasColumnName("PaymentId").HasColumnType("int");
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.Property(x => x.PaymentDate).HasColumnType("datetime2");
        }
    }

    public class DamageReportConfiguration : IEntityTypeConfiguration<DamageReport>
    {
        public void Configure(EntityTypeBuilder<DamageReport> entity)
        {
            entity.ToTable(nameof(DamageReport), "Returns");
            entity.HasKey(x => x.DamageReportId);
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.RepairCost).HasColumnType("decimal(18,2)");
        }
    }

    public class OrderAdjustmentConfiguration : IEntityTypeConfiguration<OrderAdjustment>
    {
        public void Configure(EntityTypeBuilder<OrderAdjustment> entity)
        {
            entity.ToTable(nameof(OrderAdjustment), "Sales");
            entity.HasKey(x => x.AdjustmentId);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.Property(x => x.AdjustmentDate).HasColumnType("datetime2");
        }
    }
}
