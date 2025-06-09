using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail
{
    public class UserDetailModel
    {
        public string ID { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;
        public int? OwnerID { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public Guid UserID { get; set; }
        public int RoleID { get; set; }
        public bool IsActive { get; set; }
        public bool? IsBranchAdmin { get; set; }

        //public bool IsApproved { get; set; }
        //public string NewPasswordConfirmation { get; set; } = string.Empty;
        //public string NewPassword { get; set; } = string.Empty;
        public List<ListRoleViewModel> UserRoles { get; set; } = new();
        public List<GetDropDownGeneralModels> AllowedbranchlevelModels { get; set; } = new();
        public static Expression<Func<ApplicationUser, UserDetailModel>> Projection
        {
            get
            {
                return User => new UserDetailModel
                {
                    ID = User.Id.ToString(),
                    EmployeeId = User.EmployeeId,
                    UserName = User.UserName ?? string.Empty,
                    Email = User.Email ?? string.Empty,
                    IsActive = User.IsActive,
                    PhoneNumber = User.PhoneNumber ?? string.Empty,
                    IsBranchAdmin = User.IsBranchAdmin,
                };
            }
        }
        public static UserDetailModel Create(ApplicationUser user)
        {
            return Projection.Compile().Invoke(user);
        }



        public class AllowedSecuirtyLevelsModel
        {
            public string EnglishName { get; set; } = string.Empty;
            public string DariName { get; set; } = string.Empty;
            public string PashtoName { get; set; } = string.Empty;
            public int Id { get; set; }


        }
    }
}
