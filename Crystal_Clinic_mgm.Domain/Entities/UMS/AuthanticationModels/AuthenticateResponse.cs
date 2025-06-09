using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels
{
    public class AuthenticateResponse
    {
        public Guid ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
        //   [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }

        public AuthenticateResponse(ApplicationUser user, EmployeeProfile? employee, string positionTitle, string jwtToken, string refreshToken)
        {
            ID = user.Id;
            UserName = user.UserName ?? string.Empty;
            PositionTitle = positionTitle;
            Email = user.Email ?? string.Empty;
            JwtToken = jwtToken ?? string.Empty; ;
            RefreshToken = refreshToken ?? string.Empty;
            PhotoPath = employee?.PhotoPath ?? string.Empty;
            TokenExpiration = DateTime.Now.AddHours(2);
        }
    }
}