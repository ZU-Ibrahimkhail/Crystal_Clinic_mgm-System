using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS
{
    public class AccountTrackingConfiguration : IEntityTypeConfiguration<AccountTracking>
    {
        public void Configure(EntityTypeBuilder<AccountTracking> entity)
        {
            entity.ToTable(nameof(AccountTracking), "AssetMS");
            entity.HasKey("ID");
            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(x => x.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.TransactionDate).HasColumnName("TransactionDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.UserId).HasColumnName("UserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);

            entity.Property(c => c.DebitAmount).HasColumnName("DebitAmount").HasColumnType("real");
            entity.Property(c => c.CreditAmount).HasColumnName("CreditAmount").HasColumnType("real");
            entity.Property(c => c.BalanceAmount).HasColumnName("BalanceAmount").HasColumnType("real");

            entity.Property(c => c.MainAccountId).HasColumnName("MainAccountId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.HasOne(x => x.MainAccount).WithMany().HasForeignKey(x => x.MainAccountId).OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<AccountTracking>.AuditableEntityConfigurations(entity);

        }
    }
}
