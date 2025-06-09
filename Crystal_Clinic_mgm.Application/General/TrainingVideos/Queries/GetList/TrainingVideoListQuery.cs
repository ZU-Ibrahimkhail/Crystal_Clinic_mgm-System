using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Queries.GetList
{
    public class TrainingVideoListQuery : DataTableOption, IRequest<ResponseDataTable<TrainingVideoListModel>>
    {
        public string? SearchBy { get; set; }
    }
}
