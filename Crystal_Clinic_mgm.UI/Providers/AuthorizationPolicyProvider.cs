using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
namespace Crystal_Clinic_Mgm.UI.Providers
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        readonly UMSLocalizeMessage umslocalizeMessage;
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IStringLocalizer<UMSValidationResource> umslocalizer) : base(options)
        {
            _options = options.Value;
            umslocalizeMessage = new(umslocalizer);
        }
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy == null)
            {
                if (string.IsNullOrEmpty(policyName) == false)
                {
                    if (policyName.Contains(':'))
                    {
                        var array = policyName.Split(':');
                        var type = array[0];
                        var value = array[1];
                        policy = new AuthorizationPolicyBuilder().RequireClaim(type, value).Build();
                        _options.AddPolicy(policyName, policy);
                    }
                    else
                    {
                        throw new ApplicationException(umslocalizeMessage.InvalidPolicy);
                    }
                }
                else
                {
                    throw new ApplicationException(umslocalizeMessage.InvalidPolicy);
                }
            }
            return policy;
        }
    }
}
