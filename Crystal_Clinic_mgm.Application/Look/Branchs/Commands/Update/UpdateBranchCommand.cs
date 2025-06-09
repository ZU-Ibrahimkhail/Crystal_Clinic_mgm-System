using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Update
{
    public class UpdateBranchCommand : IRequest<JsonResult>
    {

        [JsonIgnore]
        public int Id { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
