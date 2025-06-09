using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail
{
    public class GetPartnersDetailHandler : IRequestHandler<GetPartnersDetailQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Partners> _GenericRepositoryAsync;
        private readonly IMessage _message;

        public GetPartnersDetailHandler(IGenericRepositoryAsync<ERP_DbContext, Partners> genericRepositoryAsync, IMessage message)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _message = message;
        }

        public async Task<JsonResult> Handle(GetPartnersDetailQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);

            //if (request.Id > 0)
            //{
            //    List<GetPartnersDetailModel> getPartnersDetailModels = new();

            //    var branch = await _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false && x.ID == request.Id)
            //        .Select
            //        (
            //        .SingleOrDefaultAsync(cancellationToken);
            //    if (branch == null)
            //        return _message.RecordNotFound();
            //    return new JsonResult(branch);
            //}
            //else
                return _message.IdErrorMessage();
        }
    }
}
