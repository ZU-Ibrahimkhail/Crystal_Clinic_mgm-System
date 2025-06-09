using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using static Crystal_Clinic_Mgm.Common.Constants.Constants;

namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories
{
    #region Generic Helper for AuditableEntity 
    public class GeneralHelperRepositoryAsync<TContext, TEntity> : IGeneralHelperRepositoryAsync<TContext, TEntity> where TContext : DbContext where TEntity : AuditableEntity
    {
        private readonly TContext _DbContext;
        public GeneralHelperRepositoryAsync(TContext context)
        {
            _DbContext = context;
        }
        #region For Create & update Validations

        public string GetPrimaryKey<T>() where T : class => _DbContext.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties[0].Name ?? "ID";
        public bool IsUnique(object PropertyValue, string PropertyName, object? Id = null, Guid? CreatedBy = null)
        {
            try
            {
                return !_DbContext.Set<TEntity>().Any(x => x.IsDeleted == false
                        && EF.Property<object>(x, PropertyName) == PropertyValue
                        && (CreatedBy == null || x.CreatedBy != CreatedBy)
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsUnique(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null, Guid? CreatedBy = null)
        {
            try
            {
                return !_DbContext.Set<TEntity>().Any(x => x.IsDeleted == false
                        && EF.Property<object>(x, propertyName1) == PropertyValue1
                        && EF.Property<object>(x, propertyName2) == PropertyValue2
                        && (CreatedBy == null || x.CreatedBy != CreatedBy)
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsUnique(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null, Guid? CreatedBy = null)
        {
            try
            {

                return !_DbContext.Set<TEntity>().Any(x => x.IsDeleted == false
                        && EF.Property<object>(x, propertyName1) == PropertyValue1
                        && EF.Property<object>(x, propertyName2) == PropertyValue2
                        && EF.Property<object>(x, propertyName3) == PropertyValue3
                        && (CreatedBy == null || x.CreatedBy != CreatedBy)
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsUnique(object? Id = null, Guid? CreatedBy = null, params (object PropertyValue, string PropertyName)[] properties)
        {
            try
            {
                var query = _DbContext.Set<TEntity>().Where(x => !x.IsDeleted
                        && (CreatedBy == null || x.CreatedBy != CreatedBy)
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));

                foreach (var property in properties)
                {
                    query = query.Where(x => EF.Property<object>(x, property.PropertyName) == property.PropertyValue);
                }
                return !query.Any();
            }
            catch (Exception)
            {
                throw;
            }
        }


        // generic methodes
        public bool IsUnique<T>(object PropertyValue, string PropertyName, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity
        {
            try
            {

                return !_DbContext.Set<T>().Any(x => x.IsDeleted == false
                        && EF.Property<object>(x, PropertyName) == PropertyValue
                        && (CreatedBy == null || x.CreatedBy != CreatedBy)
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<T>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsUnique<T>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity
        {
            try
            {

                return !_DbContext.Set<T>().Any(x => x.IsDeleted == false
                       && EF.Property<object>(x, propertyName1) == PropertyValue1
                       && EF.Property<object>(x, propertyName2) == PropertyValue2
                       && (CreatedBy == null || x.CreatedBy != CreatedBy)
                       && (Id == null || EF.Property<object>(x, GetPrimaryKey<T>()) != Id));

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsUnique<T>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null, Guid? CreatedBy = null) where T : AuditableEntity
        {
            try
            {
                return !_DbContext.Set<T>().Any(x => x.IsDeleted == false
                            && EF.Property<object>(x, propertyName1) == PropertyValue1
                            && EF.Property<object>(x, propertyName2) == PropertyValue2
                            && EF.Property<object>(x, propertyName3) == PropertyValue3
                            && (CreatedBy == null || x.CreatedBy != CreatedBy)
                            && (Id == null || EF.Property<object>(x, GetPrimaryKey<T>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
    #endregion

    #region Non generic helper
    public class GeneralHelperRepositoryAsync : IGeneralHelperRepositoryAsync
    {
        private readonly ERP_DbContext _ERPDbContext;
        private readonly UMS_DbContext _UMSDbContext;

        public GeneralHelperRepositoryAsync(ERP_DbContext eRPDbContext, UMS_DbContext uMSDbContext)
        {
            _ERPDbContext = eRPDbContext;
            _UMSDbContext = uMSDbContext;
        }
        Localization localization = new();
        public string GetLanguageName(string language, int id)
        {
            try
            {
                var lang = _UMSDbContext.Languages.SingleOrDefault(x => x.ID == id);
                if (lang != null)
                {
                    return localization.GetName(language, lang);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetUserName(string Language, Guid? Id)
        {
            try
            {
                ApplicationUser? user = _UMSDbContext.Users.SingleOrDefault(x => x.Id == Id);
                if (user != null)
                {
                    return Language switch
                    {
                        Constants.Language.English => _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId)?.EnglishFirstName ?? user.UserName,
                        _ => _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId)?.PashtoFirstName ?? user.UserName,
                    } ?? string.Empty;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetEmployeeLocaalizeFullName(int? EmployeeId, string language)
        {
            var employee = _ERPDbContext.EmployeeProfiles.Find(EmployeeId);
            return employee != null ? language switch
            {
                Constants.Language.English => employee.EnglishFirstName + " " + employee.EnglishSurName,
                _ => employee.PashtoFirstName + " " + employee.PashtoSurName,
            } : string.Empty;
        }
        public string GetUserPhotoPath(Guid? Id)
        {
            try
            {
                var user = _UMSDbContext.Users.SingleOrDefault(x => x.Id == Id);
                return _ERPDbContext.EmployeeProfiles.Find(user?.EmployeeId)?.PhotoPath ?? string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetUserPosition(string Language, Guid? Id)
        {
            try
            {
                var user = _UMSDbContext.Users.SingleOrDefault(x => x.Id == Id);
                return GetEmployeePosition(Language, user?.EmployeeId ?? 0);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetEmployeePosition(string Language, int EmployeeId)
        {
            try
            {
                Localization Localize = new();
                var Contract = _ERPDbContext.ContractDetails.Where(x => !x.IsDeleted && x.IsActive && x.EmployeeProfileId == EmployeeId)
                    .Include(x => x.PositionTitle).FirstOrDefault();
                return Localize.GetName(Language, Contract?.PositionTitle);

            }
            catch (Exception)
            {

                throw;
            }
        }
  
        public async Task<List<string>> GetUserNameList(List<Guid> IdList, string Language)
        {
            try
            {
                List<string> NameList = new();
                var users = await _UMSDbContext.Users.Where(x => IdList.Contains(x.Id)).ToListAsync();
                if (users.Any())
                {
                    foreach (var user in users)
                    {

                        NameList.Add(Language switch
                        {
                            Constants.Language.English => _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId)?.EnglishFirstName ?? string.Empty,
                            _ => _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId)?.PashtoFirstName ?? string.Empty,
                        });
                    }
                }
                return NameList;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<int>> GetChildBranchs(int parentId, int? DelpartmentLevelIds = null, IQueryable<Branch>? BranchList = null)
        {
            BranchList ??= _ERPDbContext.Set<Branch>().Where(x => x.IsDeleted == false);
            var ch = await BranchList.Where(x => x.ID != parentId && x.ParentId == parentId).ToListAsync();
            List<int> ChildBranchList = new();
            foreach (var child in ch)
            {
                ChildBranchList.Add(child.ID);
                ChildBranchList.AddRange(await GetChildBranchs(child.ID, DelpartmentLevelIds, BranchList));
            }
            return ChildBranchList;
        }

        public async Task<Branch?> GetParentBranch(int BranchId, int DelpartmentLevelIds)
        {
            var Branch = await _ERPDbContext.Set<Branch>().Where(x => x.IsDeleted == false && x.ID == BranchId).Include(x => x.Parent).FirstOrDefaultAsync();

            if (Branch != null)
            {
                while (Branch.Parent != null && Branch?.ParentId != Branch?.ID)
                {

                    Branch = Branch!.Parent;
                }
            }
            return null;
        }
        public bool BranchCanHaveCC(int BranchID)
        {
            try
            {
                return _ERPDbContext.Branchs.Any(x => x.ID == BranchID && !x.IsDeleted);
            }
            catch (Exception)
            {

                throw;
            }
        }
      
        public string GetUserCurrentBranchName(string language, Guid? uId)
        {
            try
            {
                Localization localize = new();
                var user = _UMSDbContext.Users.SingleOrDefault(x => x.Id == uId);
                var branch = user != null ? _ERPDbContext.Branchs.Find(user.BranchId) : null;

                return localize.GetName(language, branch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Guid>> GetUsersByAccessPermission(string permissionName, int? deprtmentId = null)
        {
            try
            {
                var roleIds = await _UMSDbContext.RolePermission.Where(x => !x.IsDeleted && !x.Permission!.IsDeleted && x.Permission!.Name == permissionName).Select(x => x.RoleId).ToListAsync();
                List<Guid> users = new();
                foreach (var roleId in roleIds)
                {
                    users.AddRange(_UMSDbContext.UserRoles.Where(x => !x.IsDeleted && x.RoleId == roleId && !x.User.IsDeleted && x.User.IsActive && (deprtmentId == null || x.User.BranchId == deprtmentId)).Select(x => x.UserId).ToList());
                }
                return users;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Guid>> GetManagerUserList(int BranchID)
        {
            try
            {
                return await _UMSDbContext.Users.Where(x => !x.IsDeleted && x.IsActive && x.BranchId == BranchID).Select(x => x.Id).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<int>> GetParentBranchIds(int? BranchId)
        {
            List<int> ParentBranchIds = new();

            var entity = await _ERPDbContext.Branchs.Where(x => !x.IsDeleted && x.IsActive).ToListAsync();
            int? depId = BranchId;
            while (depId != null)
            {
                var Temp = entity.FirstOrDefault(x => x.ID == depId);
                if (Temp == null || Temp.ParentId == Temp.ID)
                {
                    break;
                }
                ParentBranchIds.Add(Temp.ID);
                depId = Temp!.ParentId;
            }
            return ParentBranchIds;
        }
    }

    #endregion

    #region General helper for all entities
    public class GeneralHelperRepositoryAsync<TContext> : IGeneralHelperRepositoryAsync<TContext> where TContext : DbContext
    {
        private readonly TContext _DbContext;
        public GeneralHelperRepositoryAsync(TContext context)
        {
            _DbContext = context;
        }
        public string GetPrimaryKey<T>() where T : class => _DbContext.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties[0].Name ?? "Id";

        public bool IsUnique<TEntity>(object PropertyValue, string propertyName, object? Id = null) where TEntity : class
        {
            try
            {
                return !_DbContext.Set<TEntity>().Any(x =>
                        EF.Property<object>(x, propertyName) == PropertyValue
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsUnique<TEntity>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object? Id = null) where TEntity : class
        {
            try
            {
                return !_DbContext.Set<TEntity>().Any(x =>
                       EF.Property<object>(x, propertyName1) == PropertyValue1
                       && EF.Property<object>(x, propertyName2) == PropertyValue2
                       && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsUnique<TEntity>(object PropertyValue1, string propertyName1, object PropertyValue2, string propertyName2, object PropertyValue3, string propertyName3, object? Id = null) where TEntity : class
        {
            try
            {
                return !_DbContext.Set<TEntity>().Any(x =>
                        EF.Property<object>(x, propertyName1) == PropertyValue1
                        && EF.Property<object>(x, propertyName2) == PropertyValue2
                        && EF.Property<object>(x, propertyName3) == PropertyValue3
                        && (Id == null || EF.Property<object>(x, GetPrimaryKey<TEntity>()) != Id));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    #endregion

}
