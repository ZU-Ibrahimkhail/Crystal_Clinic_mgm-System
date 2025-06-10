using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock.Look
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
            public void Configure(EntityTypeBuilder<Item> entity)
            {
                // Table Configuration
                entity.ToTable("Item", "Stock");

                // Primary Key
                entity.HasKey(i => i.ItemId);

                // Properties configuration
                entity.Property(i => i.Name)
                    .HasColumnName("Name")
                    .HasColumnType("nvarchar(255)")
                    .IsRequired();

                entity.Property(i => i.Description)
                    .HasColumnName("Description")
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.Property(i => i.BaseUnit)
                    .HasColumnName("BaseUnit")
                    .HasColumnType("nvarchar(50)")
                    .IsRequired();

                entity.Property(i => i.CurrentStock)
                    .HasColumnName("CurrentStock")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(i => i.UseableStock)
                    .HasColumnName("UseableStock")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired(false); // Nullable

                entity.Property(i => i.ReorderLevel)
                    .HasColumnName("ReorderLevel")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(i => i.ImagePath)
                    .HasColumnName("ImagePath")
                    .HasColumnType("nvarchar(255)")
                    .IsRequired(false); // Nullable

                // Foreign Keys Configuration
                entity.HasOne(i => i.Category)
                    .WithMany()
                    .HasForeignKey(i => i.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction); // No cascading delete

                // Optional: You can also create an index on the `Name` for search optimization
                entity.HasIndex(i => i.Name).HasDatabaseName("IX_Item_Name");

            EntityConfiguration<Item>.AuditableEntityConfigurations(entity);
            }
        }
    public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> entity)
        {
            entity.ToTable(nameof(ItemCategory), "Stock");
            entity.HasKey(x => x.categoryId);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            EntityConfiguration<ItemCategory>.AuditableEntityConfigurations(entity);

        }
    }

}
