using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetAllPermissions
{
    public class GetPermissionsListQuery : DataTableOption, IRequest<DataTableResponse>
    {
        public string? SearchBy { get; set; }
    }
}
