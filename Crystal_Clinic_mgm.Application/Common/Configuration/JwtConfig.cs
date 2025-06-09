namespace Crystal_Clinic_Mgm.Application.Common.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double DaysToExpire { get; set; }
    }
}
