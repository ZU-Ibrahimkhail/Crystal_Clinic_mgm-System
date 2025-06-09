using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Queries.GetList
{
    public class GetAdvancePaymentListQuery : DataTableOption, IRequest<ResponseDataTable<GetAdvancePaymentListModel>>
    {
        public string? SearchBy { get; set; }
        public Guid? MainAccountId { get; set; }
    }
}
