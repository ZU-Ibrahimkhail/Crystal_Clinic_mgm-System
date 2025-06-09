using Microsoft.Extensions.Localization;
namespace Crystal_Clinic_Mgm.Common.CommonLocalizations
{
    public interface ICommonValidationResource
    {
    }
    public class CommonValidationResource : ICommonValidationResource
    {
        private readonly IStringLocalizer _localizer;
        public CommonValidationResource(IStringLocalizer<CommonValidationResource> localizer)
        {
            _localizer = localizer;
        }
        public string this[string key] => _localizer.GetString(key).Value ?? "--";
    }
}
