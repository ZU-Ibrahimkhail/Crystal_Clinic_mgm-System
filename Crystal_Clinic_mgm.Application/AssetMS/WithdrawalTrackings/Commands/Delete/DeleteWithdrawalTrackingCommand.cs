using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Delete
{
    public class DeleteWithdrawalTrackingCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get;  set; }
    }
}
