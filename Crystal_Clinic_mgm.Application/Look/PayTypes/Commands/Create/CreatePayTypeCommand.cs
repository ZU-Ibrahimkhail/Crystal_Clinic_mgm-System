using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Create
{
    public class CreatePayTypeCommand : GeneralLookCreateCommand, IRequest<JsonResult>
    {
    }
}
