using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Microsoft.AspNetCore.Identity;

namespace Crystal_Clinic_Mgm.Persistence.Initializers
{
    public class UMSintializer
    {
        public async static Task InitializeUMS(UserManager<ApplicationUser> _userManager)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync("IsSuperAdmin@SuperAdmin.com");
                if (user != null)
                {
                    return;
                }
                var SuperAdmin = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "IsSuperAdmin@SuperAdmin.com",
                    IsActive = true,
                    PhoneNumber = "0730000000",
                    EmailConfirmed = true,
                    SuccessLoginCount = 0,
                    BranchId = 1,
                    IsSuperAdmin = true,
                    CreatedBy = Guid.NewGuid(),
                    ModifiedBy = Guid.NewGuid(),
                    LastLoginDate = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    RefreshToken = ""
                };
                var result = await _userManager.CreateAsync(SuperAdmin, "Crystal_ClinicMgm12345!@#$%");
                if (!result.Succeeded)
                {
                    Exception ex = new("SuperAdmin");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
