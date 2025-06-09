using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail
{
    public class GetPartnersDetailQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int Id { get; set; }
    }
}
