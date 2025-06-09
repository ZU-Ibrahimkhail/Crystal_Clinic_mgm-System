using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDDL
{
    public class GetUserDDLQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public bool ShowLoginUser { get; set; }
    }
}
