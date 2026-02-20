namespace Crystal_Clinic_Mgm.Application.HR.HR
{
    public interface ITaxCalculationService
    {
        /// <summary>
        /// Calculate tax based on database configuration
        /// </summary>
        /// <param name="taxTypeId">Tax configuration ID</param>
        /// <param name="salary">Salary amount to calculate tax on</param>
        /// <returns>Calculated tax amount</returns>
        Task<decimal> CalculateTaxAsync(int taxConfigurationId, decimal salary);

        /// <summary>
        /// Calculate tax with audit trail
        /// </summary>
        /// <param name="taxConfigurationId">Tax configuration ID</param>
        /// <param name="salary">Salary amount to calculate tax on</param>
        /// <returns>Tax calculation with details</returns>
        Task<TaxCalculationDetail> CalculateTaxWithDetailsAsync(int taxConfigurationId, decimal salary);
    }

    public class TaxCalculationDetail
    {
        public decimal TotalTax { get; set; }
        public List<BracketCalculation> Brackets { get; set; } = new List<BracketCalculation>();
        public string CalculationFormula { get; set; } = string.Empty;
    }

    public class BracketCalculation
    {
        public int BracketOrder { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal FlatAmount { get; set; }
        public decimal CalculatedTax { get; set; }
        public string Formula { get; set; } = string.Empty;
    }
}
