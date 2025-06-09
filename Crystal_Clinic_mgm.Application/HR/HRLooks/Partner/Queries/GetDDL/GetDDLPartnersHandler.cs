using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDDL
{
    public class GetPartnersDDLHandler(IGenericRepositoryAsync<ERP_DbContext, Partners> genericRepositoryAsync) : IRequestHandler<GetPartnersDDLQuery, List<GetPartnetDDLModel>>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, Partners> _GenericRepositoryAsync = genericRepositoryAsync;

        public async Task<List<GetPartnetDDLModel>> Handle(GetPartnersDDLQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            List<GetPartnetDDLModel> branchs = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted).Select(x => new GetPartnetDDLModel
            {
                Id = x.ID,
                Name = language == Constants.Language.English ? x.NameInEnglish : x.NameInPashto,
                Email = x.Email,
                Phone = x.Phone
            }).ToListAsync(cancellationToken);
            return branchs;
        }
    }
}
