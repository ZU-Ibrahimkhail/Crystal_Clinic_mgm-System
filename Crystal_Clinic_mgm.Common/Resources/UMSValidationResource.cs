using Microsoft.Extensions.Localization;
namespace Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization
{
    public interface IUMSValidationResource
    {
    }
    public class UMSValidationResource : IUMSValidationResource
    {
        private readonly IStringLocalizer _localizer;
        public UMSValidationResource(IStringLocalizer<UMSValidationResource> localizer)
        {
            _localizer = localizer;
        }
        public string this[string key] => _localizer.GetString(key).Value ?? "--";
    }
}
