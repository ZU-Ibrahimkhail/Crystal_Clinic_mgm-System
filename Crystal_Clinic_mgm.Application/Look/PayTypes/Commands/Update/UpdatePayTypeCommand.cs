using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Update
{
    public class UpdatePayTypeCommand : GeneralLookUpdateCommand, IRequest<JsonResult>
    {
    }
}
