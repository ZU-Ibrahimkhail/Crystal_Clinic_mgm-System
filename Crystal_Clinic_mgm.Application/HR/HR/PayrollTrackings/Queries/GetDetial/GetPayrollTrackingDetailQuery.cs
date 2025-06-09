using MediatR;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetDetial
{
    public class GetPayrollTrackingDetailQuery : IRequest<GetPayrollTrackingListModel>
    {
        public int Id { get; set; }
    }
}
