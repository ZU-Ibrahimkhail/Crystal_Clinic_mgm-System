using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order
{
    public class ReturnConfiguration : IEntityTypeConfiguration<Return>
    {
        public void Configure(EntityTypeBuilder<Return> entity)
        {
            entity.ToTable(nameof(Return), "Rentals");
            entity.HasKey(x => x.ReturnId);

            entity.Property(x => x.ReturnDate).HasColumnName("ReturnDate").HasColumnType("datetime2").IsRequired();
            entity.Property(x => x.DamageFee).HasColumnName("DamageFee").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.DepositRefund).HasColumnName("DepositRefund").HasColumnType("decimal(18,2)").IsRequired();

            // Relationships
            entity.HasOne(x => x.Order)
                  .WithMany()
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Employee)
                  .WithMany()
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
