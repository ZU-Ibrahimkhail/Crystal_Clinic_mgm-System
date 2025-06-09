using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using System.Linq.Expressions;
namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserInRolesDashBoard
{
    public class UserInRoleModel
    {
        public string UserName { get; set; } = string.Empty;

        public int Counts { get; set; }
        public static Expression<Func<UserRole, string, IGeneralHelperRepositoryAsync, UserInRoleModel>> Projection
        {
            get
            {

                return (tr, language, _helper) => new UserInRoleModel
                {
                    UserName = _helper.GetUserName(language, tr.UserId),
                    Counts = tr.ID
                };
            }
        }
    }


}
