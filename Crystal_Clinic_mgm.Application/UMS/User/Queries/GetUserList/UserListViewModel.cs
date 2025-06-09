namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserList
{
    public class UserListViewModel
    {
        public IList<UserListLookupModel> UsersLists { get; set; } = new List<UserListLookupModel>();
    }
}
