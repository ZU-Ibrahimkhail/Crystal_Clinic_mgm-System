using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDDL
{
    public class GetPartnersDDLQuery : IRequest<List<GetPartnetDDLModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
    }
}
