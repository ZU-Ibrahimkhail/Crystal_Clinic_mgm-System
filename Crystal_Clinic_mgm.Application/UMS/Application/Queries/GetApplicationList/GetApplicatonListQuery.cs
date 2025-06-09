using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationList
{
    public class GetApplicatonListQuery : DataTableOption, IRequest<ResponseDataTable<GetApplicationListLookupModel>>
    {
        public string Title { get; set; } = string.Empty;
    }
}
