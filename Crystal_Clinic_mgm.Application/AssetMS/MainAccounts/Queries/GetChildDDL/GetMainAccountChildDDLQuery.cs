using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl
{
    public class GetMainAccountChildDDLQuery : IRequest<JsonResult>
    {
        public Guid? MainAccountId { get; set; }
    }
}
