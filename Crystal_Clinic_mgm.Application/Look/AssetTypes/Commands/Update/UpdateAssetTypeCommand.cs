using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Update
{
    public class UpdateAssetTypeCommand : GeneralLookUpdateCommand, IRequest<JsonResult>
    {
    }
}
