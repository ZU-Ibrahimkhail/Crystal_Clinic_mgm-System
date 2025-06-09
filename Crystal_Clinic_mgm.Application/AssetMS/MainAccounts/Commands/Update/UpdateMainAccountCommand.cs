using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Update
{
    public class UpdateMainAccountCommand : IRequest<JsonResult>
    {
        public Guid ID { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime DepositDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Guid OwnerUserId { get; set; }
        public double? BalanceAmount { get; set; }
    }
}
