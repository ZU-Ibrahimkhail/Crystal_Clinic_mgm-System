using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.GetList
{
    public class GetUserAuditListQuery : DataTableOption, IRequest<ResponseDataTable<GetUserAuditListModel>>
    {
        public string? Search { get; set; }
    }
}
