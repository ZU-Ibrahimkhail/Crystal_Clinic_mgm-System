using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetDetail
{
    public class GetLoanTypeDetailHandler : IRequestHandler<GetLoanTypeDetailQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, LoanType> _GenericRepositoryAsync;
        private readonly IMessage _message;

        public GetLoanTypeDetailHandler(IGenericRepositoryAsync<ERP_DbContext, LoanType> genericRepositoryAsync, IMessage message)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _message = message;
        }

        public async Task<JsonResult> Handle(GetLoanTypeDetailQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);

            if (request.Id > 0)
            {
                List<GetLoanTypeDetailModel> getLoanTypeDetailModels = new();

                var branch = await _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false && x.ID == request.Id)
                    .Select
                    (branch => GetLoanTypeDetailModel
                    .Projection
                    .Compile().Invoke(branch, language))
                    .SingleOrDefaultAsync(cancellationToken);
                if (branch == null)
                    return _message.RecordNotFound();
                return new JsonResult(branch);
            }
            else
                return _message.IdErrorMessage();
        }
    }
}
