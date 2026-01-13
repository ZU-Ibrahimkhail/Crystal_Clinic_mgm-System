using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting;

public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
{
    public void Configure(EntityTypeBuilder<CompanyProfile> entity)
    {
        entity.ToTable(nameof(ChartOfAccounts), "Accounting");

        entity.HasKey(c => c.Id);

        entity.HasOne(x => x.CashAccount)
            .WithMany()
            .HasForeignKey(x => x.CashAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.BankAccount)
            .WithMany()
            .HasForeignKey(x => x.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.SalesRevenueAccount)
            .WithMany()
            .HasForeignKey(x => x.SalesRevenueAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.InventoryAccount)
            .WithMany()
            .HasForeignKey(x => x.InventoryAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.PurchaseExpenseAccount)
            .WithMany()
            .HasForeignKey(x => x.PurchaseExpenseAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.AccountsReceivableAccount)
            .WithMany()
            .HasForeignKey(x => x.AccountsReceivableAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.AccountsPayableAccount)
            .WithMany()
            .HasForeignKey(x => x.AccountsPayableAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
