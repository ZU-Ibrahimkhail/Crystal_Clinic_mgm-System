using AutoMapper;
using MediatR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Queries.GetList
{
    public class TrainingVideoListHandler : IRequestHandler<TrainingVideoListQuery, ResponseDataTable<TrainingVideoListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> _GRepoAsync;
        private readonly IMapper _Mapper;

        public TrainingVideoListHandler(IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> gRepoAsync, IMapper mapper)
        {
            _GRepoAsync = gRepoAsync;
            _Mapper = mapper;
        }

        public async Task<ResponseDataTable<TrainingVideoListModel>> Handle(TrainingVideoListQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var entity = _GRepoAsync.FindByCondition(x => !x.IsDeleted && (request.SearchBy == null ||
                x.DariTitle.ToLower().Contains(request.SearchBy.ToLower()) ||
                x.PashtoTitle.ToLower().Contains(request.SearchBy.ToLower()) ||
                x.Application.ToLower().Contains(request.SearchBy.ToLower())
                ));


                var records = _Mapper.Map<List<TrainingVideoListModel>>(entity);

                return MyDataTable<TrainingVideoListModel>.Generate(records, request.PageSize, request.PageIndex);
            });
        }
    }
}
