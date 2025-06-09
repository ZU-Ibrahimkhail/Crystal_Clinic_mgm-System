using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Create
{
    public class CreateCurrencyTypeCommand : GeneralLookCreateCommand, IRequest<JsonResult>
    {
    }
}
