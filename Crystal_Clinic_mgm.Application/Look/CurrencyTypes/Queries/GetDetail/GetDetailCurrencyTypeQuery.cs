using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDetail
{
    public class GetCurrencyTypeDetailQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int Id { get; set; }
    }
}
