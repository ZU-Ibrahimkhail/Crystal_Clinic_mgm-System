using AutoMapper;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.General.News.Queries.GetList;
using Crystal_Clinic_Mgm.Application.General.News.Queries.NewsDashboard;
using Crystal_Clinic_Mgm.Application.General.TrainingVideos.Queries.GetList;
using Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Queries.GetList;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.UMS.UserReport.UserAccountList.Queries;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Crystal_Clinic_Mgm.Application.Common.MappingProfiles
{
    public class GeneralMappingProfile : Profile
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public GeneralMappingProfile(IHttpContextAccessor? httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor!;
            _helper = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IGeneralHelperRepositoryAsync>();
            //IMediator Mediator = _httpContextAccessor.HttpContext.RequestServices.GetService<IMediator>()!;
            string selectedLanguage = GeneralHelper.SelectedLanauge(_httpContextAccessor.HttpContext.Request.Cookies[Constants.CultureCookies.CookiesName]);
            Localization localize = new(_httpContextAccessor);
            ERP_DbContext _dbcontext = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ERP_DbContext>();
            IStringLocalizer<CommonValidationResource> _localizer = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<CommonValidationResource>>();

            #region  DropDown General Model
            CreateMap<LookAndAuditableEntity, GetDropDownGeneralModel>().ForMember(d => d.Name, s => s.MapFrom(x => localize.GetName(x)));
            #endregion

            #region General Look list Model
            CreateMap<LookAndAuditableEntity, GeneralLookListModel>().ForMember(d => d.Name, src => src.MapFrom(x => localize.GetName(x)));
            CreateMap<LookAndAuditableEntity, LookGeneralNameModel>();
            #endregion

            #region UMS User Report
            CreateMap<ApplicationUser, UserReportModel>()
                .ForMember(c => c.Branch, s => s.MapFrom((user, dto, i, context) => localize.GetName(_dbcontext.Branchs.Find(user.BranchId))));
            #endregion

            CreateMap<News, GetNewsListModel>();
            CreateMap<News, NewsDashboardModel>();
            CreateMap<TrainingVideo, TrainingVideoListModel>();
            //CreateMap<AdministrativeForms, GetAdministrativeFormsListModel>();
            CreateMap<MainAccount, GetMainAccountDetailModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.OwnerUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.OwnerUserId)));

            #region Expense Tracking
            CreateMap<ExpenseTracking, GetExpenseTrackingDetailModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.ExpenseType, src => src.MapFrom(x => localize.GetName(x.ExpenseType)))
                .ForMember(d => d.UserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.UserId)))
                .ForMember(d => d.MainAccountUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.MainAccount!.OwnerUserId)))
                .ForMember(d => d.CreatedByUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.CreatedBy)));

            CreateMap<ExpenseTracking, GetExpenseTrackingListModel>()
               .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
               .ForMember(d => d.ExpenseType, src => src.MapFrom(x => localize.GetName(x.ExpenseType)))
               .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
               .ForMember(d => d.UserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.UserId)));
            #endregion


            #region withdrawal Tracking
            CreateMap<WithdrawalTracking, GetWithdrawalTrackingDetailModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.UserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.UserId)))
                .ForMember(d => d.MainAccountUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.MainAccount!.OwnerUserId)))
                .ForMember(d => d.CreatedByUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.CreatedBy)));

            CreateMap<WithdrawalTracking, GetWithdrawalTrackingListModel>()
               .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
               .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
               .ForMember(d => d.UserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.UserId)));
            #endregion


            CreateMap<MainAccount, GetMainAccountChildDDLModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.OwnerUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.OwnerUserId)))
                .ForMember(d => d.OwnerPhotoPath, src => src.MapFrom(x => _helper.GetUserPhotoPath(x.OwnerUserId)));

            CreateMap<MainAccount, GetMainAccountDDLModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.Code, src => src.MapFrom(x => $"{x.Code} ({x.BalanceAmount} {x.CurrencyType!.Code})"));
                
            CreateMap<PayrollTracking, GetPayrollTrackingListModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.EmployeeName, src => src.MapFrom(x => selectedLanguage == Constants.Language.English ? x.Employee!.EnglishFirstName : x.Employee!.PashtoFirstName))
                .ForMember(d => d.EmployeeSurName, src => src.MapFrom(x => selectedLanguage == Constants.Language.English ? x.Employee!.EnglishSurName : x.Employee!.PashtoSurName))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.PositionTitleId, src => src.MapFrom(x => x.ContractDetails!.PositionTitleId))
                .ForMember(d => d.PositionTitle, src => src.MapFrom(x => localize.GetName(x.ContractDetails!.PositionTitle)))
                .ForMember(d => d.PayType, src => src.MapFrom(x => localize.GetName(x.PayType)))
                .ForMember(d => d.PayedByUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.PayedBy)));

            CreateMap<AdvancePayment, GetAdvancePaymentListModel>()
                .ForMember(d => d.CurrencyType, src => src.MapFrom(x => localize.GetName(x.CurrencyType)))
                .ForMember(d => d.EmployeeName, src => src.MapFrom(x => selectedLanguage == Constants.Language.English ? x.Employee!.EnglishFirstName : x.Employee!.PashtoFirstName))
                .ForMember(d => d.EmployeeSurName, src => src.MapFrom(x => selectedLanguage == Constants.Language.English ? x.Employee!.EnglishSurName : x.Employee!.PashtoSurName))
                .ForMember(d => d.BranchId, src => src.MapFrom(x => x.Employee!.BranchId))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Employee!.Branch)))
                .ForMember(d => d.PayType, src => src.MapFrom(x => localize.GetName(x.PayType)))
                .ForMember(d => d.PayedByUserName, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.PayedBy)));


        }

        private string GetPartnerName(Partners? x, string selectedLanguage)
        {
            return selectedLanguage switch { Constants.Language.English => x?.NameInEnglish ?? string.Empty, _ => x?.NameInPashto ?? string.Empty };
        }
    }
}
