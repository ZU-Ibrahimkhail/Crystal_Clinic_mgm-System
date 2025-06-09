using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Queries.GetDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Queries.GetList
{
    public class GetExpenseTypeListQuery : DataTableOption, IRequest<ResponseDataTable<GetExpenseTypeDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
    }
}
