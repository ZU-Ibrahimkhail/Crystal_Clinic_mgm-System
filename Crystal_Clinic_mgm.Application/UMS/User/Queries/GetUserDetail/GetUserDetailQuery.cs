using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail
{
    public class GetUserDetailQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
