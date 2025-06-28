using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Create
{
    public class CreateExpenseTrackingCommand :  IRequest<JsonResult>
    {
        public Guid MainAccountId { get; set; }
        public int ExpenseTypeId { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public IFormFile? Attachment { get; set; }
    }
}
