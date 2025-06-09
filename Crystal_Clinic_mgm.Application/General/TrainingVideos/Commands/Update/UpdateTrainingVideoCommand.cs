using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Update
{
    public class UpdateTrainingVideoCommand : IRequest<JsonResult>
    {
        public int id { get; set; }
        public string DariTitle { get; set; } = string.Empty;
        public string PashtoTitle { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public IFormFile? Poster { get; set; }
        public IFormFile? DariVideoPath { get; set; }
        public IFormFile? PashtoVideoPath { get; set; }
    }
}