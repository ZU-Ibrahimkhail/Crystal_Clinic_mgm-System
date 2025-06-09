using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    #region Generic Helper for AuditableEntity
    public interface IGeneralHelperRepositoryAsync<TContext, TEntity> where TContext : DbContext where TEntity : AuditableEntity
    {
        // for Create and update validation
        public bool IsUnique(object? Id = null, Guid? CreatedBy = null, params (object PropertyValue, string PropertyName)[] properties);
        public bool IsUnique(object PropertyValue, string propertyName, object? Id = null, Guid? CreatedBy = null);
        public bool IsUnique<T>(object PropertyValue, string propertyName, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity;
        public bool IsUnique(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null, Guid? CreatedBy = null);
        public bool IsUnique<T>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity;
        public bool IsUnique(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null, Guid? CreatedBy = null);


        public bool IsUnique<T>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity;
    }
    #endregion

    #region Non Generic helper
    public interface IGeneralHelperRepositoryAsync
    {
        /// <summary>
        /// Returns the language localize name from Database
        /// </summary>
        /// <param name="id"> the Id of the Language</param>
        /// <param name="language"> the User Loging </param>
        /// <returns></returns>
        public string GetLanguageName(string language, int id);
        /// <summary>
        /// Returns the employee name of the corresponding user
        /// </summary>
        /// <param name="EmployeeId"> the Id of the Employee</param>
        /// <param name="language">required language</param>
        /// <returns></returns>
        public string GetEmployeeLocaalizeFullName(int? EmployeeId, string language);
        /// <summary>
        /// Returns the employee name of the corresponding user
        /// </summary>
        /// <param name="Language">required language</param>
        /// <param name="Id"> the Id of the user</param>
        /// <returns></returns>
        public string GetUserName(string Language, Guid? Id);

        /// <summary>
        /// returns the photo path of the user
        /// </summary>
        /// <param name="Id">user id</param>
        /// <returns></returns>
        public string GetUserPhotoPath(Guid? Id);

        /// <summary>
        /// Return the position of the employee by the corresponding user
        /// </summary>        
        /// /// <param name="Language">required language</param>
        /// <param name="Id"> the Id of the user</param>
        /// <returns></returns>
        public string GetUserPosition(string Language, Guid? Id);
        /// <summary>
        /// Returns the current position of the employee 
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public string GetEmployeePosition(string Language, int EmployeeId);


        /// <summary>
        /// Return Manager users of a branch
        /// </summary>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public Task<List<Guid>> GetManagerUserList(int BranchID);
        /// <summary>
        /// this method is used to find the users that have a specific permission.
        /// </summary>
        /// <param name="permissionName">the name of the permission</param>
        /// <param name="branchId">in order to further filter users by branch Id</param>
        /// <returns></returns>
        public Task<List<Guid>> GetUsersByAccessPermission(string permissionName, int? branchId = null);

        /// <summary>
        ///  return a list of user names corresponding to the userId lis 
        /// </summary>
        /// <param name="IdList"> list of user Ids</param>
        /// <returns></returns>
        public Task<List<string>> GetUserNameList(List<Guid> IdList, string Language);

        /// <summary>
        /// methode for getting all child branch you can not use with any dbcontext except ERP_DbContext
        /// </summary>
        /// <typeparam name="T"> is the branch</typeparam>
        /// <param name="parentId">is the id of the parent branch to search it's childs</param>
        /// <param name="DelpartmentLevelIds">is the id of the Branch Level to search the level mentioned and below that level</param>
        /// <returns></returns>
        public Task<List<int>> GetChildBranchs(int parentId, int? DelpartmentLevelIds = null, IQueryable<Branch>? BranchList = null);
        /// <summary>
        /// returns the parent of a branch that has a specific level
        /// </summary>
        /// <param name="BranchId"></param>
        /// <param name="DelpartmentLevelIds"></param>
        /// <param name="Branch"></param>
        /// <returns></returns>
        public Task<Branch?> GetParentBranch(int BranchId, int DelpartmentLevelIds);
        /// <summary>
        ///  returns the parent Ids of a branch 
        /// </summary>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public Task<List<int>> GetParentBranchIds(int? BranchId);
        public bool BranchCanHaveCC(int BranchID);
        public string GetUserCurrentBranchName(string language, Guid? uId);


    }

    #endregion

    #region General helper for all Entities
    public interface IGeneralHelperRepositoryAsync<TContext> where TContext : DbContext
    {
        public bool IsUnique<TEntity>(object PropertyValue, string propertyName, object? Id = null) where TEntity : class;
        public bool IsUnique<TEntity>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null) where TEntity : class;
        public bool IsUnique<TEntity>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null) where TEntity : class;
    }
    #endregion

}
