using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting;

public class CompanyProfile : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string WhatsappNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public CurrencyType? BaseCurrency { get; set; }
    public int CashAccountId { get; set; }
    public int BankAccountId { get; set; }
    public int AccountsReceivableAccountId { get; set; }
    public int AccountsPayableAccountId { get; set; }
    public int SalesRevenueAccountId { get; set; }
    public int InventoryAccountId { get; set; }
    public int PurchaseExpenseAccountId { get; set; }
    public bool IsInitialized { get; set; } = false;
    public ChartOfAccounts CashAccount { get; set; } = null!;
    public ChartOfAccounts BankAccount { get; set; } = null!;
    public ChartOfAccounts AccountsReceivableAccount { get; set; } = null!;
    public ChartOfAccounts AccountsPayableAccount { get; set; } = null!;
    public ChartOfAccounts SalesRevenueAccount { get; set; } = null!;
    public ChartOfAccounts InventoryAccount { get; set; } = null!;
    public ChartOfAccounts PurchaseExpenseAccount { get; set; } = null!;
}
