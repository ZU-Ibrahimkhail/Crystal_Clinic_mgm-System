using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.ViewModel;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDDL
{
    public class GetUserDDLQueryHandler : IRequestHandler<GetUserDDLQuery, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _genericRepositoryAsync;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public GetUserDDLQueryHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> genericRepositoryAsync, IGeneralHelperRepositoryAsync helper)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(GetUserDDLQuery request, CancellationToken cancellationToken)
        {
            if (request.BranchId > 0)
            {
                var language = GeneralHelper.SelectedLanauge(request.Language);
                List<UserDDLViewModel> user = await _genericRepositoryAsync.FindByCondition(x =>
                                        x.BranchId == request.BranchId && (request.ShowLoginUser || x.Id != _loggedInUser.Id) && x.UserName != "SuperAdmin" &&
                                        x.IsDeleted == false && x.IsActive == true)
                                        .Select(x => new UserDDLViewModel
                                        {
                                            Id = x.Id,
                                            UserName = _helper.GetUserName(language, x.Id),
                                            PositionTitle = _helper.GetUserPosition(language, x.Id),
                                            Email = x.Email ?? string.Empty,

                                        }).ToListAsync(cancellationToken);
                if (user != null)
                    return new JsonResult(user);
                else
                    return _message.RecordNotFound();
            }
            else
            {
                return _message.IdErrorMessage(request.BranchId);
            }
        }
    }
}
