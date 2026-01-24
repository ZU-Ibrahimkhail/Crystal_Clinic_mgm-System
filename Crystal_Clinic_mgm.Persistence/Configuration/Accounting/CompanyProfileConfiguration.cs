using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Accounting;

public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
{
    public void Configure(EntityTypeBuilder<CompanyProfile> builder)
    {
        builder.ToTable(nameof(CompanyProfile), "Accounting");

        builder.HasKey(c => c.Id);


        builder.Property(c => c.Name)
               .HasColumnName("Name")
               .HasColumnType("nvarchar(200)")
               .IsRequired();

        builder.Property(c => c.Email)
               .HasColumnName("Email")
               .HasColumnType("nvarchar(150)")
               .IsRequired(false);

        builder.Property(c => c.PhoneNumber)
               .HasColumnName("PhoneNumber")
               .HasColumnType("nvarchar(50)")
               .IsRequired(false);

        builder.Property(c => c.WhatsappNumber)
               .HasColumnName("WhatsappNumber")
               .HasColumnType("nvarchar(50)")
               .IsRequired(false);

        builder.Property(c => c.Description)
               .HasColumnName("Description")
               .HasColumnType("nvarchar(max)")
               .IsRequired(false);

        builder.Property(c => c.CashAccountId)
               .HasColumnName("CashAccountId");

        builder.Property(c => c.BankAccountId)
               .HasColumnName("BankAccountId");

        builder.Property(c => c.AccountsReceivableAccountId)
               .HasColumnName("AccountsReceivableAccountId");

        builder.Property(c => c.AccountsPayableAccountId)
               .HasColumnName("AccountsPayableAccountId");

        builder.Property(c => c.SalesRevenueAccountId)
               .HasColumnName("SalesRevenueAccountId");

        builder.Property(c => c.InventoryAccountId)
               .HasColumnName("InventoryAccountId");

        builder.Property(c => c.PurchaseExpenseAccountId)
               .HasColumnName("PurchaseExpenseAccountId");

        builder.Property(c => c.IsInitialized)
               .HasColumnName("IsInitialized")
               .HasColumnType("bit")
               .HasDefaultValue(false);

        builder.Property(c => c.BaseCurrencyId)
                .HasColumnName("BaseCurrencyId")
                .IsRequired(false);

        builder.HasOne<CurrencyType>("CurrencyType")
               .WithMany()
               .HasForeignKey(c => c.BaseCurrencyId)
               .OnDelete(DeleteBehavior.NoAction);

        // Chart of Accounts
        builder.HasOne(c => c.CashAccount)
               .WithMany()
               .HasForeignKey(c => c.CashAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.BankAccount)
               .WithMany()
               .HasForeignKey(c => c.BankAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.AccountsReceivableAccount)
               .WithMany()
               .HasForeignKey(c => c.AccountsReceivableAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.AccountsPayableAccount)
               .WithMany()
               .HasForeignKey(c => c.AccountsPayableAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.SalesRevenueAccount)
               .WithMany()
               .HasForeignKey(c => c.SalesRevenueAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.InventoryAccount)
               .WithMany()
               .HasForeignKey(c => c.InventoryAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.PurchaseExpenseAccount)
               .WithMany()
               .HasForeignKey(c => c.PurchaseExpenseAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        EntityConfiguration<CompanyProfile>.AuditableEntityConfigurations(builder);
    }
}
