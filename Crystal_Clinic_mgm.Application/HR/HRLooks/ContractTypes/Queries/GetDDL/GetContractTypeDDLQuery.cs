using MediatR;
using Newtonsoft.Json;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetDDL
{
    public class GetContractTypeDDLQuery : IRequest<List<GetDropDownGeneralModel>>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
    }
}
