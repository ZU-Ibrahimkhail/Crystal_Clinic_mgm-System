using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail
{
    public class GetWithdrawalTrackingDetailQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
