using Microsoft.IdentityModel.Tokens;
using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crystal_Clinic_Mgm.Application.Common.Identity
{
    /// <summary>
    /// Token provider which is used to generate token with different option configuration and also return token with user detail like name,email,phone no etc 
    /// </summary>
    public class TokenProvider
    {
        private readonly JwtConfig _jwtConfig;
        public TokenProvider(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public async Task<JwtSecurityToken> BuildToken(ApplicationUser user, EmployeeProfile? employee, int UserAuditTableId)
        {
            return await Task.Run(() =>
            {
                List<Claim> claims = new()
            {
               new Claim("ITEmployeeName",employee?.EnglishFirstName ??string.Empty),
               new Claim("ITEmployeeDariName",employee?.PashtoFirstName?? string.Empty),
               new Claim("UserName",user.UserName ?? string.Empty),
               new Claim("UserId",user.Id.ToString()),
               new Claim("UserAuditTableId",UserAuditTableId.ToString()),
               new Claim("Email", user.Email ?? string.Empty),
               new Claim("PhotoPath", employee?.PhotoPath ?? string.Empty),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
                var token = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    expires: DateTime.Now.AddHours(2),
                    claims: claims,
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256));
                return token;
            });
        }
        public RefreshToken GenerateRefreshToken(ApplicationUser user, EmployeeProfile? employee, string ipAddress, int UserAuditTableId)
        {
            var claims = new List<Claim>()
            {
                new("ITEmployeeName",employee?.EnglishFirstName ?? string.Empty),
                new("ITEmployeeDariName",employee?.PashtoFirstName?? string.Empty),
                new("UserName",user.UserName ?? string.Empty),
                new("UserId",user.Id.ToString()),
                new("UserAuditTableId",  UserAuditTableId.ToString()),
                new("Email", user.Email ?? string.Empty),
                new("PhotoPath", employee?.PhotoPath ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );
            var RefreshToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new RefreshToken
            {
                Token = RefreshToken,
                Expires = DateTime.Now.AddHours(2),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };
        }
    }
}
