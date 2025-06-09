using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Delete
{
    public class DeleteNewsCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
