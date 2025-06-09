using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList
{
    public class GetPayrollTrackingListQuery : DataTableOption, IRequest<ResponseDataTable<GetPayrollTrackingListModel>>
    {
        public string? SearchBy { get; set; }
        public int? BranchId { get; set; }
    }
}
