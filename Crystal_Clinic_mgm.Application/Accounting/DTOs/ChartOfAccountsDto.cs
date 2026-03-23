using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class ChartOfAccountsDto
    {
        public int Id { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public string AccountTypeName => AccountType.ToString();
        public AccountCategory AccountCategory { get; set; }
        public string AccountCategoryName => AccountCategory.ToString();
        public NormalBalanceType NormalBalance { get; set; }
        public bool IsSystemAccount { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
    }

    public class CreateChartOfAccountsDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public AccountCategory AccountCategory { get; set; }
        public NormalBalanceType NormalBalance { get; set; }
        public string? Description { get; set; }
        public int? ParentAccountId { get; set; }
    }

    public class UpdateChartOfAccountsDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
