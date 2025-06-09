using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.ExpenseTypes.Queries.GetList
{
    public class GetExpenseTypeListHandler(IGenericRepositoryAsync<ERP_DbContext, ExpenseType> genericRepositoryAsync) : IRequestHandler<GetExpenseTypeListQuery, ResponseDataTable<GetExpenseTypeDetailModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseType> _GenericRepositoryAsync = genericRepositoryAsync;

        public async Task<ResponseDataTable<GetExpenseTypeDetailModel>> Handle(GetExpenseTypeListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            string? SearchBy = request.SearchBy?.ToLower().Trim();
            var entity = _GenericRepositoryAsync
                .FindByCondition(x => x.IsDeleted == false && (SearchBy == null || 
                x.EnglishName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) || 
                x.PashtoName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) || 
                x.DariName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) || 
                x.Code.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase)))
                .OrderByDescending(x => x.ModifiedOn)
                .Select(entity => GetExpenseTypeDetailModel.Projection.Compile().Invoke(entity, language));

            return await MyDataTable<GetExpenseTypeDetailModel>.Generate(entity, request.PageSize, request.PageIndex);

        }
    }
}
