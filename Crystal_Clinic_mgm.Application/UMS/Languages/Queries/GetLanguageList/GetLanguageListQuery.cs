using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguageList
{
    public class GetLanguageListQuery : DataTableOption, IRequest<ResponseDataTable<GetDropDownGeneralModel>>
    {
        public string? Name { get; set; }
    }
}
