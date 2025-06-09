using Microsoft.Extensions.Localization;
namespace Crystal_Clinic_Mgm.Common.CommonResponseLocalization
{
    public interface ICommonResponseResource
    {
    }
    public class CommonResponseResource : ICommonResponseResource
    {
        private readonly IStringLocalizer _localizer;
        public CommonResponseResource(IStringLocalizer<CommonResponseResource> localizer)
        {
            _localizer = localizer;
        }
        public string this[string key] => _localizer.GetString(key).Value ?? "--";
    }
 
}
