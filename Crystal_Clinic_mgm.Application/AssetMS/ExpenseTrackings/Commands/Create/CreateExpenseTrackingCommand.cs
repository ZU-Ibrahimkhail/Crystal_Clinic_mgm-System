using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Create
{
    public class CreateExpenseTrackingCommand :  IRequest<JsonResult>
    {
        public Guid MainAccountId { get; set; }
        public int ExpenseTypeId { get; set; }
        //public int CurrencyTypeId { get; set; }
        public float Amount { get; set; }
        //public double CRateToMainAccountCurrencyType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
