using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock
{
    public class VisitKitsConfiguration : IEntityTypeConfiguration<VisitKits>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<VisitKits> entity)
        {
            entity.ToTable(nameof(VisitKits), "VisitKits");

            entity.HasKey(vk => vk.Id);

            entity.Property(vk => vk.VisitId)
                .HasColumnName("VisitId")
                .IsRequired();

            entity.Property(vk => vk.ServiceSessionId)
                .HasColumnName("ServiceSessionId")
                .IsRequired(false);

            entity.Property(vk => vk.KitId)
                .HasColumnName("KitId")
                .IsRequired();

            entity.Property(vk => vk.IsConsumed)
                .HasColumnName("IsConsumed")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(vk => vk.InventoryKit)
                .WithMany()
                .HasForeignKey(vk => vk.KitId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting InventoryKit if VisitKits exists

            entity.HasOne(vk => vk.Visit)
                .WithMany()
                .HasForeignKey(vk => vk.VisitId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Visit if VisitKits exists

            entity.HasOne(vk => vk.ServiceSessions)
                .WithMany()
                .HasForeignKey(vk => vk.ServiceSessionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting ServiceSessions if VisitKits exists

            entity.HasIndex(vk => new { vk.VisitId, vk.ServiceSessionId, vk.KitId }).IsUnique();

            EntityConfiguration<VisitKits>.AuditableEntityConfigurations(entity);
        }
    }
}
