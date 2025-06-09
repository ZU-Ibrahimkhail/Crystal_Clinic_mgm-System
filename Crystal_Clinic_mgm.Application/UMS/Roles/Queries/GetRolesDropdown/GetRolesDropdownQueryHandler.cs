using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRolesDropdown
{
    public class GetRolesDropdownQueryHandler : IRequestHandler<GetRolesDropdownQuery, JsonResult>
    {
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;

        public GetRolesDropdownQueryHandler(IMessage message, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole)
        {
            _message = message;
            _GRepoRole = gRepoRole;
        }

        public async Task<JsonResult> Handle(GetRolesDropdownQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var Roles = await _GRepoRole.FindByCondition(x => x.IsDeleted == false)
                                    .Select(r => new
                                    {
                                        r.Id,
                                        r.Name,
                                        r.ApplicationId,
                                        r.IsDeleted
                                    })
                                    .ToListAsync(cancellationToken);
                return new JsonResult(Roles);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.Message.ToString());
            }
        }
    }
}
