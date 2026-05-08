using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Create
{
    public class CreateNewsCommand : IRequest<JsonResult>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime NewsDate { get; set; }
        public string? Speaker { get; set; }
        public string? Location { get; set; }
        public bool? ShowNotification { get; set; }
        public List<string>? Attachment { get; set; } = [];
        public string? BranchIds { get; set; } = string.Empty;
    }
}
