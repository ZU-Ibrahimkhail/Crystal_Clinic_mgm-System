using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.UpdateDeposit
{
    public class UpdateDepositCommand :  IRequest<JsonResult>
    {
        public int ID { get; set; }
        public Guid MainAccountId { get; set; }
        //public int CurrencyTypeId { get; set; }
        public double DepositAmount { get; set; }
        //public double CRateToMainAccountCurrencyType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
