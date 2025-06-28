using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetList
{
    public class GetPositionTitleListQueryHandler(IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync, IMapper mapper) : IRequestHandler<GetPositionTitleListQuery, ResponseDataTable<GetPositionTitleListModel>>
    {
        public async Task<ResponseDataTable<GetPositionTitleListModel>> Handle(GetPositionTitleListQuery request, CancellationToken cancellationToken)
        {
            var GradeStep = await Task.Run(() =>
            {
                string? searchBy = null;
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    searchBy = request.Name.ToLower().Trim();
                }
                return genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false
                                    &&
                                    (searchBy == null ||
                                    x.EnglishName.ToLower().Contains(searchBy) ||
                                    x.PashtoName.ToLower().Contains(searchBy) ||
                                    x.DariName.ToLower().Contains(searchBy) ||
                                    x.Code.ToLower().Contains(searchBy)
                                    )).OrderByDescending(x => x.ModifiedOn)
                                    .Include(x => x.Branch);
            });



            var entity = mapper.Map<List<GetPositionTitleListModel>>(GradeStep);

            var result = MyDataTable<GetPositionTitleListModel>.Generate(entity, request.PageSize, request.PageIndex);
            return result;
        }
    }
}
