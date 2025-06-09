using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDDL
{
    public class GetMainAccountDDLQuery : IRequest<JsonResult>
    {
        public Guid? UserId { get; set; }
    }
}
