using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetList
{
    public class GetPositionTitleListQuery : DataTableOption, IRequest<ResponseDataTable<GetPositionTitleListModel>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
