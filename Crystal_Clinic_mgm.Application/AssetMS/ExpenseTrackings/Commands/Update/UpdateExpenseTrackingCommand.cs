using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update
{
    public class UpdateExpenseTrackingCommand :  IRequest<JsonResult>
    {
        public int ID { get; set; }
        public Guid MainAccountId { get; set; }
        public int ExpenseTypeId { get; set; }
        //public int CurrencyTypeId { get; set; }
        public float Amount { get; set; }
        //public double CRateToMainAccountCurrencyType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
