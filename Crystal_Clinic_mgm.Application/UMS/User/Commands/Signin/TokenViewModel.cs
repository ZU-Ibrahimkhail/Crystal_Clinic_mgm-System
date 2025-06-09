namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class TokenViewModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
