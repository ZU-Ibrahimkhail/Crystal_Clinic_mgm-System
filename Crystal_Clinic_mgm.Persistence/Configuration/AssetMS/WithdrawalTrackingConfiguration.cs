using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS
{
    public class WithdrawalTrackingConfiguration : IEntityTypeConfiguration<WithdrawalTracking>
    {
        public void Configure(EntityTypeBuilder<WithdrawalTracking> entity)
        {
            entity.ToTable(nameof(WithdrawalTracking), "AssetMS");
            entity.HasKey("ID");
            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(x => x.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.MainAccountId).HasColumnName("MainAccountId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne(x => x.MainAccount).WithMany().HasForeignKey(x => x.MainAccountId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.WithdrawalAmount).HasColumnName("WithdrawalAmount").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.DepositAmount).HasColumnName("DepositAmount").HasColumnType("real").IsRequired(true);
            entity.Property(c => c.Date).HasColumnName("Date").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);

            EntityConfiguration<WithdrawalTracking>.AuditableEntityConfigurations(entity);

        }
    }
}
