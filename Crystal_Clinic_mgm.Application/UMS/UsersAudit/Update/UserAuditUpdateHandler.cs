using MediatR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.Update
{
    public class UserAuditUpdateHandler : IRequestHandler<UserAuditUpdateCommand, bool>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAudit> _GenericRepository;

        public UserAuditUpdateHandler(IGenericRepositoryAsync<UMS_DbContext, UserAudit> genericRepository)
        {
            _GenericRepository = genericRepository;
        }

        public async Task<bool> Handle(UserAuditUpdateCommand request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var entity = _GenericRepository.FindByCondition(x => x.UserId == request.UserId
                                && x.ActionOn.Date == DateTime.Now.Date
                                // && x.IpAddress == request.IpAddress
                                // && x.BrowserName == request.BrowserName
                                // && x.BrowserVersion == request.BrowserVersion
                                // && x.ActionEnd == null
                                //&& x.ActionOn > DateTime.Now.AddDays(-1)
                                ).OrderByDescending(x => x.ActionOn).FirstOrDefault();

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    entity.ActionEnd = DateTime.Now;
                    _GenericRepository.EditeAsync(entity, cancellationToken);
                    return true;
                }
            });


        }
    }
}
