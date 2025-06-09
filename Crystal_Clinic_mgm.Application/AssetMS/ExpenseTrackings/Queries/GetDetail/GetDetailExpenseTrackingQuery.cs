using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail
{
    public class GetExpenseTrackingDetailQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
