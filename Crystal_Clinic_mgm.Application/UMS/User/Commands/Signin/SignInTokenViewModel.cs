namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class SignInTokenViewModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }
    }
}
