using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Create
{
    public class CreateMainAccountCommand :  IRequest<JsonResult>
    {
        public int CurrencyTypeId { get; set; }
        public DateTime DepositDate { get; set; }
        public int BranchId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid OwnerUserId { get; set; }
        public double BalanceAmount { get; set; }
    }
}
