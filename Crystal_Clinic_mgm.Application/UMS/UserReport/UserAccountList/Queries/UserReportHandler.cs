using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserAccountList.Queries
{
    public class UserReportHandler : IRequestHandler<UserReportQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public UserReportHandler(IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<JsonResult> Handle(UserReportQuery request, CancellationToken cancellationToken)
        {
            var language = GeneralHelper.SelectedLanauge(request.Language);

            var entity = await _genericRepositoryAsync.FindByCondition(x => !x.IsDeleted
            &&
            (request.BranchId == null || x.BranchId == request.BranchId)
            &&
            (request.IsActive == null || request.IsActive == x.IsActive)
            ).ToListAsync();
            var userReport = _mapper.Map<IEnumerable<UserReportModel>>(entity);

            return new JsonResult(userReport);


        }
    }
}
