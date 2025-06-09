using System.Security.Cryptography;
using System.Text;
namespace Crystal_Clinic_Mgm.Application.Common.Identity
{
    public class CreatePasswordHash
    {
        public CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

    }
}
