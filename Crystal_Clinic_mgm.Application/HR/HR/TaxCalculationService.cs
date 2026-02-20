using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.HR.HR
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly ERP_DbContext _context;

        public TaxCalculationService(ERP_DbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateTaxAsync(int taxConfigurationId, decimal salary)
        {
            var details = await CalculateTaxWithDetailsAsync(taxConfigurationId, salary);
            return details.TotalTax;
        }

        public async Task<TaxCalculationDetail> CalculateTaxWithDetailsAsync(int taxConfigurationId, decimal salary)
        {
            var taxConfig = await _context.TaxConfigurations
                .Include(t => t.Brackets)
                .FirstOrDefaultAsync(t => t.Id == taxConfigurationId && t.IsActive);

            if (taxConfig == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            var detail = new TaxCalculationDetail();
            var activeBrackets = taxConfig.Brackets
                .Where(b => b.IsActive)
                .OrderBy(b => b.BracketOrder)
                .ToList();

            if (!activeBrackets.Any())
                return detail;

            foreach (var bracket in activeBrackets)
            {
                if (salary <= 0)
                    break;

                var bracketCalculation = new BracketCalculation
                {
                    BracketOrder = bracket.BracketOrder,
                    MinAmount = bracket.MinAmount,
                    MaxAmount = bracket.MaxAmount,
                    Percentage = bracket.Percentage,
                    FlatAmount = bracket.FlatAmount
                };

                if (bracket.CalculationType == TaxCalculationType.Single)
                {
                    if (salary >= bracket.MinAmount && salary <= bracket.MaxAmount)
                    {
                        if (bracket.Percentage > 0)
                        {
                            bracketCalculation.AppliedAmount = salary;
                            bracketCalculation.CalculatedTax = salary * (bracket.Percentage / 100);
                            bracketCalculation.Formula = $"{salary} * {bracket.Percentage}% = {bracketCalculation.CalculatedTax}";
                        }
                        else
                        {
                            bracketCalculation.AppliedAmount = 1;
                            bracketCalculation.CalculatedTax = bracket.FlatAmount;
                            bracketCalculation.Formula = $"Flat Amount = {bracketCalculation.CalculatedTax}";
                        }

                        detail.Brackets.Add(bracketCalculation);
                        detail.TotalTax += bracketCalculation.CalculatedTax;
                        break;
                    }
                }
                else if (bracket.CalculationType == TaxCalculationType.StepByStep)
                {
                    if (salary > bracket.MinAmount)
                    {
                        var appliedAmount = 0m;

                        if (salary >= bracket.MaxAmount)
                        {
                            appliedAmount = bracket.MaxAmount - bracket.MinAmount;
                        }
                        else
                        {
                            appliedAmount = salary - bracket.MinAmount;
                        }

                        if (appliedAmount > 0)
                        {
                            bracketCalculation.AppliedAmount = appliedAmount;

                            if (bracket.Percentage > 0)
                            {
                                bracketCalculation.CalculatedTax = appliedAmount * (bracket.Percentage / 100);
                                bracketCalculation.Formula = $"({bracket.MaxAmount} - {bracket.MinAmount}) * {bracket.Percentage}% = {bracketCalculation.CalculatedTax}";
                            }
                            else
                            {
                                bracketCalculation.CalculatedTax = bracket.FlatAmount;
                                bracketCalculation.Formula = $"Flat Amount = {bracketCalculation.CalculatedTax}";
                            }

                            detail.Brackets.Add(bracketCalculation);
                            detail.TotalTax += bracketCalculation.CalculatedTax;
                        }

                        if (salary < bracket.MaxAmount)
                            break;
                    }
                }
            }

            detail.CalculationFormula = string.Join(" + ", detail.Brackets.Select(b => b.Formula));
            return detail;
        }
    }
}
