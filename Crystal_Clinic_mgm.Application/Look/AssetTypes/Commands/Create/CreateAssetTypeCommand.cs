using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Create
{
    public class CreateAssetTypeCommand : GeneralLookCreateCommand, IRequest<JsonResult>
    {
    }
}
