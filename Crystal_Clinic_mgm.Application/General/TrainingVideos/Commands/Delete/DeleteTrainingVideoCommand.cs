using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Delete
{
    public class DeleteTrainingVideoCommand : IRequest<JsonResult>
    {
        public int Id { get; set; }
        public string? Remark { get; set; }
    }
}
