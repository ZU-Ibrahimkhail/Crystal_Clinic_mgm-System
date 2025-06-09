using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDetails
{
    public class GetEmployeeProfileDetailsHandler : IRequestHandler<GetEmployeeProfileDetailsQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IMessage _message;
        private readonly IMapper _Mapper;
        public GetEmployeeProfileDetailsHandler(IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee, IMessage message, IMapper mapper)
        {
            _GRepoEmployee = gRepoEmployee;
            _message = message;
            _Mapper = mapper;
        }

        public async Task<JsonResult> Handle(GetEmployeeProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            var entity = await _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.ID == request.Id)
                .Include(x => x.Branch)
                .SingleOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                return _message.RecordNotFound();
            }
            return new JsonResult(_Mapper.Map<GetEmployeeProfileDetailsModel>(entity));
        }
    }
}
