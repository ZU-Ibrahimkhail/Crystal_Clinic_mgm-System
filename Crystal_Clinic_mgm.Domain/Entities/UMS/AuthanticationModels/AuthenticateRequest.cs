namespace Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels
{
    public class AuthenticateRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}