using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList
{
    public class GetRoleListQuery : DataTableOption, IRequest<DataTableResponse>
    {
        public string? SearchBy { get; set; }
    }
}
