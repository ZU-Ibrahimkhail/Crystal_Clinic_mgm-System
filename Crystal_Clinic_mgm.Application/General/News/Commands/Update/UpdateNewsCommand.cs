using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Update
{
    public class UpdateNewsCommand : IRequest<JsonResult>
    {

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime NewsDate { get; set; }
        public string? Speaker { get; set; }
        public string? Location { get; set; }
        public IFormFile? Attachment { get; set; }


    }
}
