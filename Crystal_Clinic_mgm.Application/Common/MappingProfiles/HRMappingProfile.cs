using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
//using MediatR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDetails;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileForEdit;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileList;
using Crystal_Clinic_Mgm.Application.HR.HR.Report.Queries;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Application.Common.MappingProfiles
{
    public class HRMappingProfile : Profile
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public HRMappingProfile(IHttpContextAccessor? httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor!;
            _helper = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IGeneralHelperRepositoryAsync>();
            //IMediator Mediator = _httpContextAccessor.HttpContext.RequestServices.GetService<IMediator>()!;
            string selectedLanguage = GeneralHelper.SelectedLanauge(_httpContextAccessor.HttpContext.Request.Cookies[Constants.CultureCookies.CookiesName]);
            Localization localize = new(_httpContextAccessor);

            #region Employee Profile

            CreateMap<EmployeeProfile, GetEmployeeModel>()
                .ForMember(d => d.EnglishName, src => src.MapFrom(x => x.EnglishFirstName + " " + x.EnglishSurName))
                .ForMember(d => d.DariFatherName, src => src.MapFrom(x => x.PashtoFatherName))
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.Position, src => src.MapFrom(x => _helper.GetEmployeePosition(selectedLanguage, x.ID)))
                .ForMember(d => d.Email, src => src.MapFrom(x => x.PersonalEmail))
                .ForMember(d => d.DariName, src => src.MapFrom(x => x.PashtoFirstName + " " + x.PashtoSurName));

            CreateMap<EmployeeProfile, GetEmployeeProfileListModel>()
                .ForMember(d => d.BranchName, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.Name, src => src.MapFrom(x => localize.GetEmployeeLocalizedName(x, true)))  //<= IsFirstName = True
                .ForMember(d => d.SurName, src => src.MapFrom(x => localize.GetEmployeeLocalizedName(x, false)))  //<= IsFirstName = false 
                .ForMember(d => d.CreatedBy, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.CreatedBy))) //<= IsFirstName = false 
                .ForMember(d => d.ModifiedBy, src => src.MapFrom(x => _helper.GetUserName(selectedLanguage, x.ModifiedBy ?? Guid.Empty)));  //<= IsFirstName = false 

            CreateMap<EmployeeProfile, GetEmployeeProfileDetailsModel>()
                .ForMember(d => d.Branch, src => src.MapFrom((entity, dto, member, context) => context.Mapper.Map<GetDropDownGeneralModel?>(entity.Branch)));


            CreateMap<EmployeeProfile, GetEmployeeProfileForEditModel>();

            #endregion

            #region Employee Report
            CreateMap<EmployeeProfile, EmployeeReportModel>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ID))
                .ForMember(x => x.firstName, s => s.MapFrom(x => localize.GetEmployeeLocalizedName(x, true)))
                .ForMember(x => x.surName, s => s.MapFrom(x => localize.GetEmployeeLocalizedName(x, false)))
                .ForMember(x => x.fullName, s => s.MapFrom(x =>
                localize.GetEmployeeLocalizedName(x, true) + " " + localize.GetEmployeeLocalizedName(x, false)))
                .ForMember(x => x.fatherName, s => s.MapFrom(x => localize.GetEmployeeLocalizeFatherName(x)))
                .ForMember(x => x.grandFatherName, s => s.MapFrom(x => localize.GetEmployeeLocalizeGrandFatherName(x)))
                .ForMember(x => x.branchName, s => s.MapFrom(x => localize.GetName(x.Branch)));

            #endregion
            #region Contract Details
            CreateMap<ContractDetails, GetContractDetailsListModel>()
                 .ForMember(d => d.EmployeeName, src => src.MapFrom(x => localize.GetEmployeeLocalizedName(x.EmployeeProfile, true) + " " + localize.GetEmployeeLocalizedName(x.EmployeeProfile, false)))
                 .ForMember(d => d.ContractType, src => src.MapFrom(x => localize.GetName(x.ContractType)))
                 .ForMember(d => d.PositionTitle, src => src.MapFrom(x => localize.GetName(x.PositionTitle)))
                 .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)));

            #endregion




            #region Position Title
            CreateMap<PositionTitle, GetPositionTitleListModel>()
                .ForMember(d => d.Branch, src => src.MapFrom(x => localize.GetName(x.Branch)))
                .ForMember(d => d.Name, src => src.MapFrom(x => localize.GetName(x)));

            CreateMap<PositionTitle, GetPositionTitleDDLModel>().ForMember(d => d.Name, src => src.MapFrom(x => localize.GetName(x)));

            #endregion
        }

        #region Helper Methodes 
        public async Task<Branch> GetParentBranch(IGeneralHelperRepositoryAsync _helper, Branch branch)
        {
            var dep = await _helper.GetParentBranch(branch.ID, Constants.BranchLevels.Directorate) ?? branch;
            return dep;
        }
        #endregion

    }
}
