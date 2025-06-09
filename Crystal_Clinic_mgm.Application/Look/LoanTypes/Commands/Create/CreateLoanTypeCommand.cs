using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.Look.LoanTypes.Commands.Create
{
    public class CreateLoanTypeCommand : GeneralLookCreateCommand, IRequest<JsonResult>
    {
    }
}
