using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.UsersAudit.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Application.Common.MappingProfiles
{
    public class UMSMappingProfile : Profile
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public UMSMappingProfile(IHttpContextAccessor? httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _helper = _httpContextAccessor!.HttpContext!.RequestServices.GetRequiredService<IGeneralHelperRepositoryAsync>();
            string selectedLanguage = GeneralHelper.SelectedLanauge(_httpContextAccessor.HttpContext.Request.Cookies[Constants.CultureCookies.CookiesName]);
            #region UMS Models
            CreateMap<UserAudit, GetUserAuditListModel>()
                .ForMember(d => d.Branch, s => s.MapFrom(x => _helper.GetUserCurrentBranchName(selectedLanguage, x.UserId)));
            #endregion
        }
    }
}
