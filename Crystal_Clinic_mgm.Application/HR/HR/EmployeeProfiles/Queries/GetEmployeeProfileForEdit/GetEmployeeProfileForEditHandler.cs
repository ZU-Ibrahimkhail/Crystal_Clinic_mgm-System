using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileForEdit
{
    public class GetEmployeeProfileForEditHandler : IRequestHandler<GetEmployeeProfileForEditQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IMessage _message;
        private readonly IMapper _mapper;
        public GetEmployeeProfileForEditHandler(IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee, IMessage message, IMapper mapper)
        {
            _GRepoEmployee = gRepoEmployee;
            _message = message;
            _mapper = mapper;
        }

        public async Task<JsonResult> Handle(GetEmployeeProfileForEditQuery request, CancellationToken cancellationToken)
        {
            var entity = await _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.ID == request.Id)
            .SingleOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                return _message.RecordNotFound();
            }
            return new JsonResult(_mapper.Map<GetEmployeeProfileForEditModel>(entity));
        }
    }
}
