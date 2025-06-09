using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Commands.Create
{
    public class CreateExpenseTypeCommand : GeneralLookCreateCommand, IRequest<JsonResult>
    {
    }
}
