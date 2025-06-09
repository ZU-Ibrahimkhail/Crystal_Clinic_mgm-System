using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetDetail
{
    public class GetLoanTypeDetailQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int Id { get; set; }
    }
}
