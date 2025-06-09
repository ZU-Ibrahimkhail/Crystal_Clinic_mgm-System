using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Commands.Delete
{
    public class DeleteExpenseTypeCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get;  set; }
    }
}
