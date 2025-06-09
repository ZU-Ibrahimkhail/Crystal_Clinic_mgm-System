using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Delete
{
    public class DeleteAssetTypeCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get;  set; }
    }
}
