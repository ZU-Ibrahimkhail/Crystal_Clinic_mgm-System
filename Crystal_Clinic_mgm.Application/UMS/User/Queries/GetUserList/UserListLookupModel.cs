using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserList
{
    public class UserListLookupModel
    {
        public Guid ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        //public string PasswordConfirmation { get; set; } = string.Empty;
        public int OwnerID { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int TotalRoles { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public bool? IsBranchAdmin { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public static Expression<Func<ApplicationUser, UMS_DbContext, ERP_DbContext, IGeneralHelperRepositoryAsync, string, UserListLookupModel>> Projection
        {
            get
            {
                Localization Localize = new();
                return (user, _context, _erpdbcontext, _helper, language) => new UserListLookupModel
                {
                    ID = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    PositionTitle = _helper.GetUserPosition(language, user.Id),
                    Email = user.Email ?? string.Empty,
                    IsActive = user.IsActive,
                    OwnerID = user.EmployeeId ?? 0,
                    OwnerName = _helper.GetUserName(language, user.Id),
                    PhotoPath = _helper.GetUserPhotoPath(user.Id),
                    BranchId = user.BranchId,
                    BranchName = Localize.GetName(language, _erpdbcontext.Branchs.Find(user.BranchId)),
                    CreatedBy = user.CreatedBy,
                    CreatedByName = _helper.GetUserName(language, user.CreatedBy),
                    ModifiedOn = user.ModifiedOn,
                    TotalRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id).Count(),
                    IsBranchAdmin = user.IsBranchAdmin,

                };
            }
        }
    }
}
