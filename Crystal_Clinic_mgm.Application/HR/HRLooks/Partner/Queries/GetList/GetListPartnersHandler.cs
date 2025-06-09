using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetList
{
    public class GetPartnersListHandler(IGenericRepositoryAsync<ERP_DbContext, Partners> genericRepositoryAsync) : IRequestHandler<GetPartnersListQuery, ResponseDataTable<GetPartnersDetailModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, Partners> _GenericRepositoryAsync = genericRepositoryAsync;

        public async Task<ResponseDataTable<GetPartnersDetailModel>> Handle(GetPartnersListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            string? SearchBy = request.SearchBy?.ToLower().Trim();
            var entity = _GenericRepositoryAsync
                .FindByCondition(x => x.IsDeleted == false && (SearchBy == null ||
                x.NameInEnglish.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                x.NameInPashto.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                x.Phone.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                x.Email!.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase)))
                .OrderByDescending(x => x.ModifiedOn)
                .Select(entity => GetPartnersDetailModel.Projection.Compile().Invoke(entity, language));

            return await MyDataTable<GetPartnersDetailModel>.Generate(entity, request.PageSize, request.PageIndex);

        }
    }
}
