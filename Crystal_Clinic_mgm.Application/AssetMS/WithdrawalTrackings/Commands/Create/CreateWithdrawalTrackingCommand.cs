using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Create
{
    public class CreateWithdrawalTrackingCommand :  IRequest<JsonResult>
    {
        public Guid MainAccountId { get; set; }
        //public int CurrencyTypeId { get; set; }
        public double WithdrawalAmount { get; set; }
        //public double CRateToMainAccountCurrencyType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
