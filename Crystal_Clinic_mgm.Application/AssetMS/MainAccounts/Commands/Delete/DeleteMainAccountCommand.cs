using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Delete
{
    public class DeleteMainAccountCommand : IRequest<JsonResult>
    {
        public Guid ID { get; set; }
        public string? Remarks { get; set; }
    }
}
