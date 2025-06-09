using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Create
{
    public class CreateTrainingVideoCommand : IRequest<JsonResult>
    {
        public string DariTitle { get; set; } = string.Empty;
        public string PashtoTitle { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public IFormFile? Poster { get; set; }
        public IFormFile? DariVideoPath { get; set; }
        public IFormFile? PashtoVideoPath { get; set; }
    }
}
