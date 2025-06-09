using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail
{
    public class GetMainAccountDetailQuery : IRequest<JsonResult>
    {
        public Guid Id { get; set; }
    }
}
