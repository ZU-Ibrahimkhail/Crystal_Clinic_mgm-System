using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentList
{
    public class GetBranchListHandler : IRequestHandler<GetBranchListQuery, ResponseDataTable<GetBranchDetailModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;

        public GetBranchListHandler(IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
        }

        public async Task<ResponseDataTable<GetBranchDetailModel>> Handle(GetBranchListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            var branch = _GenericRepositoryAsync
                .FindByCondition(x => x.IsDeleted == false && x.IsActive && (request.Id == null || x.ParentId == request.Id))
                .Include(x => x.Parent)
                .Select(branch => GetBranchDetailModel.Projection.Compile().Invoke(branch, language)).AsEnumerable();


            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                string SearchBy = request.Name.ToLower().Trim();
                branch = branch.Where(x => x.EnglishName.ToLower().Contains(SearchBy) || x.PashtoName.ToLower().Contains(SearchBy) || x.DariName.ToLower().Contains(SearchBy));
                var orderby = branch.OrderByDescending(x => x.ModifiedOn);
                var res = MyDataTable<GetBranchDetailModel>.Generate(orderby, request.PageSize, request.PageIndex);
                return res;

            }
            return await Task.Run(() =>
            {
                var orderbybranch = branch.OrderByDescending(x => x.ModifiedOn);
                var result = MyDataTable<GetBranchDetailModel>.Generate(orderbybranch, request.PageSize, request.PageIndex);
                return result;
            });

        }
    }
}
