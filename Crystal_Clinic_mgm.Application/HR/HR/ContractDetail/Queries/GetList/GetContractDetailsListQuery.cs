using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList
{
    public class GetContractDetailsListQuery : DataTableOption, IRequest<ResponseDataTable<GetContractDetailsListModel>>
    {
        public string? SearchBy { get; set; }
        [JsonIgnore]
        public int EmployeeProfileId { get; set; }
    }
}
