using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositToUser
{
    public class DepositMainAccountToUserCommand : IRequest<JsonResult>
    {
        public Guid ParentId { get; set; }
        public DateTime DepositDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid? MainAccountId { get; set; }
        public double DepositAmmount { get; set; }
        public int? AssetTypeId { get; set; }
    }
}
