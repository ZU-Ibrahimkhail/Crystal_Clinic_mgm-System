using Microsoft.Extensions.Localization;
namespace Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization
{
    public interface ICommonColumnNameResource
    {
    }
    public class CommonColumnNameResource : ICommonColumnNameResource
    {
        private readonly IStringLocalizer _localizer;
        public CommonColumnNameResource(IStringLocalizer<CommonColumnNameResource> localizer)
        {
            _localizer = localizer;
        }
        public string this[string key] => _localizer.GetString(key).Value ?? "--";
    }
}
