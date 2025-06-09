using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetUnreadNotifactionCount
{
    public class GetUnreadNotifactionCountHandler : IRequestHandler<GetUnreadNotifactionCountQuery, JsonResult>
    {
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Notification> _repositoryAsync;

        public GetUnreadNotifactionCountHandler(ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, Domain.Entities.UMS.Notification> repositoryAsync)
        {
            _loggedInUser = loggedInUser;
            _repositoryAsync = repositoryAsync;
        }

        public async Task<JsonResult> Handle(GetUnreadNotifactionCountQuery request, CancellationToken cancellationToken)
        {
            var UnreadNotification = await _repositoryAsync.FindByCondition(x => x.IsDeleted == false && x.IsRead == false && x.ToUserId == _loggedInUser.Id).CountAsync(cancellationToken);

            return new JsonResult(UnreadNotification);

        }
    }
}