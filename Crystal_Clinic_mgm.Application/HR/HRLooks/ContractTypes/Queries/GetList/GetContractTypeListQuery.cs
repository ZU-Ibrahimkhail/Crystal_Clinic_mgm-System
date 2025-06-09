using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetList
{
    public class GetContractTypeListQuery : DataTableOption, IRequest<ResponseDataTable<GeneralLookListModel>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
