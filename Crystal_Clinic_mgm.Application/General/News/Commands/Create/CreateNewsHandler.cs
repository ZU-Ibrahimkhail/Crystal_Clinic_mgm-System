using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.SignalR;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;


namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Create
{
    public class CreateNewsHandler : IRequestHandler<CreateNewsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext,Crystal_Clinic_Mgm.Domain.Entities.General.News> _GRepoNews;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoUsers;
        private readonly IGenericRepositoryAsync<ERP_DbContext, NewsNotification> _GRepoNewsNotify;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IMessageHubClient _signal;

        public CreateNewsHandler(
            IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.General.News> genericRepositoryAsync,
            IGenericRepositoryAsync<ERP_DbContext, NewsNotification> nNgenericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IMessageHubClient signal,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoUsers)
        {
            _GRepoNews = genericRepositoryAsync;
            _GRepoNewsNotify = nNgenericRepositoryAsync;
            _Localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _message = message;
            _loggedInUser = loggedInUser;
            _signal = signal;
            _GRepoUsers = gRepoUsers;
        }

        public async Task<JsonResult> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateNewsValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;

            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #region Create News
            string FilePath = "";
            if (request.Attachment != null && request.Attachment.FileName.Length > 0)
            {
                FilePath = await new FileHandler().CreateAsync(request.Attachment!.OpenReadStream(),
                                  Path.GetExtension(request.Attachment.FileName), "wwwroot", AppConfig.NewsAttachments);

            }

            var entity = new Crystal_Clinic_Mgm.Domain.Entities.General.News
            {
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                NewsDate = request.NewsDate,
                Speaker = request.Speaker,
                Location = request.Location,
                ShowNotification = request.ShowNotification,
                AttachmentPath = FilePath,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            _GRepoNews.SaveAsync(entity, cancellationToken);
            #endregion
            #region Create NewsNotification
            var branchs = request.BranchIds?.Split(",").ToList() ?? new List<string>();
            List<int> branchIds = new();
            // adding _logged in users branch if not in list
            if (branchIds.Count > 0 && !branchIds.Contains(_loggedInUser.BranchId))
            {
                branchIds.Add(_loggedInUser.BranchId);
            }
            foreach (var Branch in branchs)
            {
                branchIds.Add(int.Parse(Branch));
                var newsNotification = new NewsNotification
                {
                    NewsId = entity.ID,
                    BranchId = int.Parse(Branch),
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                _GRepoNewsNotify.SaveAsync(newsNotification, cancellationToken);
            }

            if (branchIds.Count == 0)
            {
                var newsNotification = new NewsNotification
                {
                    NewsId = entity.ID,
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                _GRepoNewsNotify.SaveAsync(newsNotification, cancellationToken);
                await _signal.NewsBroadCost(entity.ID, entity.Title);
            }
            else
            {
                List<Guid>? userIds = _GRepoUsers.FindByCondition(x => x.IsActive && x.Id != _loggedInUser.Id && !x.IsDeleted && branchIds.Contains(x.BranchId ?? 0))
                                .Select(x => x.Id).ToList();
                await _signal.NewsBroadCost(entity.ID, entity.Title, userIds);

            }
            return _message.Saved();
            #endregion
        }
    }
}
