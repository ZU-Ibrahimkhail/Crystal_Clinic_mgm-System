namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs;

public class CompanyProfileDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string WhatsappNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public int CashAccountId { get; set; }
    public int BankAccountId { get; set; }
    public int AccountsReceivableAccountId { get; set; }
    public int AccountsPayableAccountId { get; set; }
    public int SalesRevenueAccountId { get; set; }
    public int InventoryAccountId { get; set; }
    public int PurchaseExpenseAccountId { get; set; }
    public int? EquityAccountId { get; set; }
    public int? FixedAssetAccountId { get; set; }
    public int? DepreciationExpenseAccountId { get; set; }
    public int? AccumulatedDepreciationAccountId { get; set; }
}