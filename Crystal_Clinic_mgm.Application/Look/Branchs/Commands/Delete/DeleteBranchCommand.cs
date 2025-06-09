using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Delete
{
    public class DeleteBranchCommand : IRequest<JsonResult>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
