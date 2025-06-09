using MediatR;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetDetial
{
    public class GetEmployeeCurrentContractQuery : IRequest<GetContractDetailsListModel>
    {
        public int EmployeeProfileId { get; set; }
    }
}
