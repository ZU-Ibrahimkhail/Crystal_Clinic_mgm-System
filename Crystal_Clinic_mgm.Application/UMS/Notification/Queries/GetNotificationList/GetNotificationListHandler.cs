using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetNotificationList
{
    public class GetNotificationListHandler : IRequestHandler<GetNotificationListQuery, ResponseDataTable<GetNotificationViewModel>>
    {
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Domain.Entities.UMS.Notification> _umsRepository;
        private readonly IGeneralHelperRepositoryAsync _helper;
        public GetNotificationListHandler(ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, Domain.Entities.UMS.Notification> umsRepository, IGeneralHelperRepositoryAsync helper)
        {
            _loggedInUser = loggedInUser;
            _umsRepository = umsRepository;
            _helper = helper;
        }

        public async Task<ResponseDataTable<GetNotificationViewModel>> Handle(GetNotificationListQuery request, CancellationToken cancellationToken)
        {
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            var NotificationList = _umsRepository.FindByCondition(x => x.ToUserId == _loggedInUser.Id && x.IsDeleted == false)
                                                 .Include(x => x.NotificationMessage)
                                                 .Include(x => x.FromUser)
                                                 .Include(x => x.ToUser)
                                                 .Include(x => x.Application)
                                                 .OrderByDescending(x => x.CreatedOn)
                                                 .Select(x => GetNotificationViewModel.Projection.Compile().Invoke(x, _helper, Language));
            var result = await MyDataTable<GetNotificationViewModel>.Generate(NotificationList, request.PageSize, request.PageIndex);
            return result;
        }
    }
}