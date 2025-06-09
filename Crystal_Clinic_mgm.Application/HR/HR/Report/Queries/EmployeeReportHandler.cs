using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Report.Queries
{
    public class EmployeeReportHandler : IRequestHandler<EmployeeReportQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _genericRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IMessage _message;
        public EmployeeReportHandler(
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> genericRepositoryAsync,
            IMapper mapper,
            IMessage message
            )
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
            _message = message;
        }

        public async Task<JsonResult> Handle(EmployeeReportQuery request, CancellationToken cancellationToken)
        {
            var language = GeneralHelper.SelectedLanauge(request.Language);

            var entity = await _genericRepositoryAsync.FindByCondition(x => !x.IsDeleted
            &&
            (request.HasAccount == null || request.HasAccount == x.HasAccount)
            ).Include(x => x.Branch)
            .ToListAsync(cancellationToken);
            var EmployeeReport = _mapper.Map<IEnumerable<EmployeeReportModel>>(entity);
            return new JsonResult(EmployeeReport);

        }
    }
}
