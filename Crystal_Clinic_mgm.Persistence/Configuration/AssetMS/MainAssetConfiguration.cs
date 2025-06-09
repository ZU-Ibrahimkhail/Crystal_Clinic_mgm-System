using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS
{
    public class MainAccountConfiguration : IEntityTypeConfiguration<MainAccount>
    {
        public void Configure(EntityTypeBuilder<MainAccount> entity)
        {
            entity.ToTable(nameof(MainAccount), "AssetMS");
            entity.HasKey(pk => pk.ID);
            entity.Property(c => c.ID).HasColumnName("ID").HasColumnType("UNIQUEIDENTIFIER");
            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.CurrencyType).WithMany().HasForeignKey(x => x.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.OwnerUserId).HasColumnName("OwnerUserId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(true);
            entity.Property(c => c.DepositDate).HasColumnName("DepositDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.Code).HasColumnName("Code").HasColumnType("nvarchar(50)").IsRequired(false);
            entity.Property(c => c.TotalDebitAmount).HasColumnName("TotalDebitAmount").HasColumnType("real");
            entity.Property(c => c.TotalCreditAmount).HasColumnName("TotalCreditAmount").HasColumnType("real");
            entity.Property(c => c.BalanceAmount).HasColumnName("BalanceAmount").HasColumnType("real");
            entity.Property(c => c.ParentId).HasColumnName("ParentId").HasColumnType("UNIQUEIDENTIFIER").IsRequired(false);
            entity.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.NoAction);
            //entity.Property(c => c.AssetTypeId).HasColumnName("AssetTypeId").HasColumnType("int").IsRequired(false);
            //entity.HasOne(x => x.AssetType).WithMany().HasForeignKey(x => x.AssetTypeId).OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<MainAccount>.AuditableEntityConfigurations(entity);

        }
    }
}
