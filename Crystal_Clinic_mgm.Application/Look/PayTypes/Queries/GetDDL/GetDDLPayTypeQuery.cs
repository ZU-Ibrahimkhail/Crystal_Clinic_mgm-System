using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetDDL
{
    public class GetPayTypeDDLQuery : IRequest<List<GetDropDownGeneralModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
    }
}
